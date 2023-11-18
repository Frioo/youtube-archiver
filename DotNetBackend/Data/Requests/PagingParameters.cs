namespace DotNetBackend.Data.Requests
{
    public class PagingParameters : IPagingParameters
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 50;
    }

    public interface IPagingParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Offset => Page * PageSize;
    }
}
