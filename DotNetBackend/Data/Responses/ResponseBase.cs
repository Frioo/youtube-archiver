using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DotNetBackend.Data.Responses
{
    public class ResponseBase
    {
        public bool IsSuccess { get; set; } = true;
        public List<string> Errors { get; set; }

        public ResponseBase(string error)
        {
            IsSuccess = false;
            Errors = new List<string> { error };
        }

        public ResponseBase(List<string> errors)
        {
            Errors = errors;
        }
    }
}
