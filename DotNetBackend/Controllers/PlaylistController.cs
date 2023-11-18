using Dapper;
using Dapper.CQRS;
using DotNetBackend.Data;
using DotNetBackend.Data.Commands;
using DotNetBackend.Data.DTO;
using DotNetBackend.Data.Queries;
using DotNetBackend.Data.Requests;
using DotNetBackend.Data.Responses;
using DotNetBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;

namespace DotNetBackend.Controllers
{
    [ApiController]
    [Route("playlist")]
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
            this.connection = connection;
            this.queryExec = queryExec;
            this.commandExec = commandExec;
            this.ytdlp = youtubeDl;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("videos")]
        public async Task<ActionResult> GetPlaylistVideos([FromQuery] GetPlaylistVideosRequest request)
        {
            var validation = await new GetPlaylistVideosRequestValidator().ValidateAsync(request);
            if (!validation.IsValid)
            {
                return BadRequest(validation);
            }

            if (string.IsNullOrWhiteSpace(request.Id))
            {
                var uri = new Uri(request.Url, UriKind.Absolute);
                request.Id = HttpUtility.ParseQueryString(uri.Query).Get("list");
            }

            
            var query = new GetPlaylistVideosPagedListQuery(request);
            using (var reader = await connection.QueryMultipleAsync(query.Sql, request))
            {
                var playlist = await reader.ReadFirstAsync<PlaylistDTO>();
                var videoCount = await reader.ReadFirstAsync<int>();
                var videos = await reader.ReadAsync<Video>();
                var totalPages = (decimal) videoCount / request.PageSize;

                var dto = new PlaylistVideosDTO
                {
                    Id = playlist.Id,
                    Title = playlist.Title,
                    Description = playlist.Description,
                    Videos = videos.ToList()
                };

                var model = new PagedResponse<PlaylistVideosDTO>(dto)
                {
                    TotalItems = videoCount,
                    TotalPages = (int) Math.Ceiling(totalPages),
                    Page = request.Page,
                    PageSize = request.PageSize,
                };
                
                return Ok(model);
            }
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
                Description = res.Data.Description,
                CreatedAt = DateTime.Now,
                LastCheckedAt = DateTime.Now,
                UpdatedAt = res.Data.ModifiedDate.HasValue ? res.Data.ModifiedDate.Value : DateTime.Now,
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
                TotalCount = videos.Count,
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
