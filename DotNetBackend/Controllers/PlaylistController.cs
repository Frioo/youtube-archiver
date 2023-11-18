using Dapper.CQRS;
using DotNetBackend.Data;
using DotNetBackend.Data.Commands;
using DotNetBackend.Data.DTO;
using DotNetBackend.Data.Requests;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YoutubeDLSharp;

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
            // Validate request
            // ...

            var res = await ytdlp.RunVideoDataFetch(PlaylistUrl(req.Id));
            if (!res.Success)
            {
                return Problem(res);
            }

            // Create playlist record
            var dto = new CreatePlaylistDTO
            {
                Id = req.Id,
                Title = res.Data.Title,
            };
            commandExec.Execute(new CreatePlaylistCommand(dto));

            if (!req.SaveVideos || res.Data.Entries.Length == 0)
            {
                return Ok();
            }

            // Create related video records
            var videos = res.Data.Entries.ToList();
            var failedVideos = new List<string>();
            using (IDbConnection conn = Database.CreateDatabaseConnection())
            {
                for (var i = 0; i < videos.Count; i++)
                {
                    var vd = videos[i];

                    if (string.IsNullOrWhiteSpace(vd.ChannelID))
                    {
                        // Skip unavailable video
                        failedVideos.Add(vd.ID);
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
            

            


            return Ok();
        }

        private string PlaylistUrl(string id) => $"https://www.youtube.com/playlist?list={id}";
    }
}
