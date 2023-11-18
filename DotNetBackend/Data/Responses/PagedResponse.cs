using DotNetBackend.Data.Requests;
using System.Collections.Generic;

namespace DotNetBackend.Data.Responses
{
    public class PagedResponse<T> : ResponseBase, IPagingParameters
    {
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 50;
        public int Offset => Page * PageSize;

        public PagedResponse(string error) : base(error)
        {
        }

        public PagedResponse(List<string> errors) : base(errors)
        {
        }

        public PagedResponse(T data) : base(data)
        {
        }
    }
}
