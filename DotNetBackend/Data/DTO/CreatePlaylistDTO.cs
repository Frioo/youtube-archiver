using System;

namespace DotNetBackend.Data.DTO
{
    public class CreatePlaylistDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LastCheckedAt { get; set; }
    }
}
