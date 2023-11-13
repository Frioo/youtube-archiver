namespace DotNetBackend.Data.Requests
{
    public class SavePlaylistRequest
    {
        public string Id { get; set; }
        public bool SaveVideos { get; set; } = true;
    }
}
