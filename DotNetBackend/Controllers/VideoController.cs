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
using System.Linq;
using YoutubeDLSharp.Metadata;

namespace DotNetBackend.Controllers
{
    [ApiController]
    [Route("video")]
    public class VideoController : BaseController
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
        public async Task<ActionResult> GetJson([FromQuery] GetVideoDataRequest request)
        {
            var validation = await new GetVideoDataRequestValidator().ValidateAsync(request);
            if (!validation.IsValid)
            {
                Debug.WriteLine($"[/video/json] request validation failed\n{request.Id}\n{request.Url}\n{string.Join('\n', validation.Errors)}");
                return BadRequest(validation);
            }

            var res = await FetchVideoData(request);
            
            if (!res.Success)
            {
                return Problem(res);
            }
            
            var videoData = res.Data;
            return Json(videoData);
        }

        [HttpGet("save")]
        public async Task<ActionResult> SaveVideo([FromQuery] GetVideoDataRequest request)
        {
            var validation = await new GetVideoDataRequestValidator().ValidateAsync(request);
            if (!validation.IsValid)
            {
                return BadRequest(validation);
            }

            var res = await FetchVideoData(request);

            if (!res.Success)
            {
                return Problem(res);
            }

            var videoData = res.Data;
            var dto = CreateVideoDTO.FromVideoData(videoData);
            var command = new CreateVideoCommand(dto);
            await CommandExecutor.ExecuteAsync(command);

            return Ok(dto.Id);
        }

        private async Task<RunResult<VideoData>> FetchVideoData(GetVideoDataRequest req)
        {
            if (string.IsNullOrEmpty(req.Url))
            {
                req.Url = VideoUrl(req.Id);
            }
            var res = await YoutubeDL.RunVideoDataFetch(req.Url);

            return res;
        }

        private string VideoUrl(string videoId) => $"https://www.youtube.com/watch?v={videoId}";
    }
}
