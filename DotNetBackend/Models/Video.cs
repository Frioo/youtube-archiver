using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetBackend.Models
{
    public class Video
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelHandle { get; set; }
        public double Duration { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
