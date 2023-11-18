namespace DotNetBackend.Data.Requests
{
    public class RequestBase : IPagingParameters
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 50;
        public int Offset => Page * PageSize;

        public RequestBase()
        {

        }

        public RequestBase(IPagingParameters pagingParameters)
        {
            Page = pagingParameters.Page;
            PageSize = pagingParameters.PageSize;
        }
    }
}
