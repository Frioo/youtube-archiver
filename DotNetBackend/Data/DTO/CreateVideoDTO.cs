using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using YoutubeDLSharp.Metadata;

namespace DotNetBackend.Data.DTO
{
    public class CreateVideoDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri ThumbnailUri { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelHandle { get; set; }
        public float? Duration { get; set; }
        public DateTime CreatedAt { get; set; }

        public static CreateVideoDTO FromVideoData(VideoData videoData)
        {
            var dto = new CreateVideoDTO()
            {
                Id = videoData.ID,
                Title = videoData.Title,
                Description = videoData.Description,
                ThumbnailUri = new Uri(videoData.Thumbnail),
                ChannelId = videoData.ChannelID,
                ChannelName = videoData.Channel,
                ChannelHandle = videoData.UploaderID,
                Duration = videoData.Duration,
                CreatedAt = DateTime.Now
            };

            if (dto.ChannelId == null)
            {
                Debug.WriteLine($"NULL CHANNEL\n{dto.Id} {dto.Title}\n{JsonSerializer.Serialize(videoData)}");
            }

            return dto;
        }
    }
}
