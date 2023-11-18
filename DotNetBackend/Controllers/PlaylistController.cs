﻿using Dapper.CQRS;
using DotNetBackend.Data;
using DotNetBackend.Data.Commands;
using DotNetBackend.Data.DTO;
using DotNetBackend.Data.Requests;
using DotNetBackend.Data.Responses;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;

namespace DotNetBackend.Controllers
{
    [ApiController]
    [Route("playlists")]
    public class PlaylistController : BaseController
    {
        private IDbConnection connection;
        private IQueryExecutor queryExec;
        private ICommandExecutor commandExec;
        private YoutubeDL ytdlp;

        public PlaylistController(
            IDbConnection connection,
            IQueryExecutor queryExec,
            ICommandExecutor commandExec,
            YoutubeDL youtubeDl
        )
        {
            this.queryExec = queryExec;
            this.commandExec = commandExec;
            this.ytdlp = youtubeDl;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("save")]
        public async Task<ActionResult> Save([FromQuery] SavePlaylistRequest req)
        {
            var validation = await new SavePlaylistRequestValidator().ValidateAsync(req);
            if (!validation.IsValid)
            {
                return BadRequest(validation);
            }

            var res = await FetchPlaylistData(req);
            if (!res.Success)
            {
                return Problem(res);
            }

            if (string.IsNullOrEmpty(req.Id))
            {
                req.Id = res.Data.ID;
            }

            // Create playlist record
            var dto = new CreatePlaylistDTO
            {
                Id = res.Data.ID,
                Title = res.Data.Title,
            };
            commandExec.Execute(new CreatePlaylistCommand(dto));

            if (!req.SaveVideos || res.Data.Entries.Length == 0)
            {
                return Ok(new SavePlaylistResponse(dto));
            }

            // Create related video records
            var videos = res.Data.Entries.ToList();
            var failedVideoIds = new List<string>();
            using (IDbConnection conn = Database.CreateDatabaseConnection())
            {
                for (var i = 0; i < videos.Count; i++)
                {
                    var vd = videos[i];

                    if (string.IsNullOrWhiteSpace(vd.ChannelID))
                    {
                        // Skip unavailable video
                        failedVideoIds.Add(vd.ID);
                        continue;
                    }

                    var v = CreateVideoDTO.FromVideoData(vd);
                    var pv = new CreatePlaylistVideoDTO() { PlaylistId = req.Id, VideoId = vd.ID };

                    var t = conn.BeginTransaction();
                    commandExec.Execute(new CreateVideoCommand(v));
                    commandExec.Execute(new CreatePlaylistVideoCommand(pv));
                    t.Commit();
                }
            }

            var model = new SavePlaylistWithVideosResponse(dto)
            {
                SuccessCount = videos.Count - failedVideoIds.Count,
                ErrorCount = failedVideoIds.Count,
                UnavailableVideos = failedVideoIds,
            };

            return Ok(model);
        }

        [HttpGet("json")]
        public async Task<ActionResult> GetPlaylistJson([FromQuery] GetPlaylistDataRequest req)
        {
            var validationResult = await new GetPlaylistDataRequestValidator().ValidateAsync(req);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult);
            }

            var res = await FetchPlaylistData(req);
            if (!res.Success)
            {
                return Problem(res);
            }

            return Ok(res.Data);
        }

        private async Task<RunResult<VideoData>> FetchPlaylistData(GetPlaylistDataRequest req)
        {
            if (string.IsNullOrEmpty(req.Url))
            {
                req.Url = PlaylistUrl(req.Id);
            }

            var res = await ytdlp.RunVideoDataFetch(req.Url);

            return res;
        }

        private string PlaylistUrl(string id) => $"https://www.youtube.com/playlist?list={id}";
    }
}
