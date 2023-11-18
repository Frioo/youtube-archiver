using DotNetBackend.Data.DTO;
using System.Collections.Generic;

namespace DotNetBackend.Data.Responses
{
    public class SavePlaylistResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public SavePlaylistResponse()
        {

        }

        public SavePlaylistResponse(CreatePlaylistDTO dto)
        {
            Id = dto.Id;
            Title = dto.Title;
        }
    }

    public class SavePlaylistWithVideosResponse : SavePlaylistResponse
    {
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<string> UnavailableVideos { get; set; }

        public SavePlaylistWithVideosResponse()
        {

        }

        public SavePlaylistWithVideosResponse(CreatePlaylistDTO dto) : base(dto)
        {
            
        }
    }
}
