using System.Diagnostics;
using System.Text.Json;
using YoutubeDLSharp.Metadata;

namespace DotNetBackend.Data.DTO
{
    public class CreateVideoDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelHandle { get; set; }
        public float? Duration { get; set; }

        public static CreateVideoDTO FromVideoData(VideoData videoData)
        {
            var dto = new CreateVideoDTO()
            {
                Id = videoData.ID,
                Title = videoData.Title,
                Description = videoData.Description,
                ChannelId = videoData.ChannelID,
                ChannelName = videoData.Channel,
                ChannelHandle = videoData.UploaderID,
                Duration = videoData.Duration
            };

            if (dto.ChannelId == null)
            {
                Debug.WriteLine($"NULL CHANNEL\n{dto.Id} {dto.Title}\n{JsonSerializer.Serialize(videoData)}");
            }

            return dto;
        }
    }
}
