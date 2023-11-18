using DotNetBackend.Data.Responses;
using Microsoft.AspNetCore.Mvc;
using YoutubeDLSharp.Metadata;
using YoutubeDLSharp;
using FluentValidation.Results;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DotNetBackend.Controllers
{
    public class BaseController : Controller
    {
        public static BadRequestObjectResult BadRequest(ValidationResult validationResult)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            var model = new ResponseBase(errors);

            return new BadRequestObjectResult(model);
        }

        public static ObjectResult Problem(RunResult<VideoData> runResult)
        {
            var errorText = string.Join('\n', runResult.ErrorOutput);
            var model = new ResponseBase(errorText);
            var res = new ObjectResult(model) { StatusCode = 500 };

            return res;
        }

        public override OkObjectResult Ok([ActionResultObjectValue] object value)
        {
            var model = new ResponseBase(value);
            var res = new OkObjectResult(model) { StatusCode = 200 };

            return res;
        }
    }
}
