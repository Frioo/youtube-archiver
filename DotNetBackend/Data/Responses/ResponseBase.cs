using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DotNetBackend.Data.Responses
{
    public class ResponseBase
    {
        public bool IsSuccess => Errors == null || Errors.Count == 0;
        public List<string> Errors { get; set; }
        public object Data { get; set; }

        public ResponseBase(string error)
        {
            Errors = new List<string> { error };
        }

        public ResponseBase(List<string> errors)
        {
            Errors = errors;
        }

        public ResponseBase(object data)
        {
            Data = data;
        }
    }
}
