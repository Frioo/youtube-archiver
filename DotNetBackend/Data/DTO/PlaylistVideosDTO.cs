using DotNetBackend.Models;
using System.Collections.Generic;

namespace DotNetBackend.Data.DTO
{
    public class PlaylistVideosDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Video> Videos { get; set; }
    }
}
