using Dapper.CQRS;
using DotNetBackend.Data.Commands;
using DotNetBackend.Data.Queries;
using DotNetBackend.Data.DTO;
using DotNetBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using YoutubeDLSharp;
using DotNetBackend.Data.Requests;
using DotNetBackend.Data.Responses;

namespace DotNetBackend.Controllers
{
    [ApiController]
    [Route("video")]
    public class VideoController : Controller
    {
        public ICommandExecutor CommandExecutor { get; }
        public IQueryExecutor QueryExecutor { get; }
        private YoutubeDL YoutubeDL { get; }

        public VideoController(
            ICommandExecutor commandExecutor,
            IQueryExecutor queryExecutor,
            YoutubeDL ytdl
        )
        {
            CommandExecutor = commandExecutor;
            QueryExecutor = queryExecutor;
            YoutubeDL = ytdl;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Video>> Get([FromQuery] GetVideoPagedListRequest request) 
        {
            var query = new GetVideoPagedListQuery(request);
            var videos = QueryExecutor.Execute(query);
            return videos;
        }

        [HttpGet("json")]
        public async Task<ActionResult> GetJson([FromQuery] string videoId)
        {
            if (string.IsNullOrWhiteSpace(videoId))
            {
                return BadRequest(new ResponseBase("Video ID is required"));
            }

            var url = VideoUrl(videoId);
            var res = await YoutubeDL.RunVideoDataFetch(url);
            
            if (!res.Success)
            {
                throw new Exception(string.Join('\n', res.ErrorOutput));
            }
            
            var videoData = res.Data;
            return Json(videoData);
        }

        [HttpGet("save")]
        public async Task<ActionResult> SaveVideo([FromQuery] string videoId)
        {
            if (string.IsNullOrWhiteSpace(videoId))
            {
                return BadRequest(new ResponseBase("Video ID is required"));
            }

            var url = VideoUrl(videoId);
            var res = await YoutubeDL.RunVideoDataFetch(url);

            if (!res.Success)
            {
                throw new Exception(string.Join('\n', res.ErrorOutput));
            }

            var videoData = res.Data;
            var dto = CreateVideoDTO.FromVideoData(videoData);
            var command = new CreateVideoCommand(dto);
            CommandExecutor.Execute(command);

            return Ok(videoId);
        }

        private string VideoUrl(string videoId) => $"https://www.youtube.com/watch?v={videoId}";
    }
}
