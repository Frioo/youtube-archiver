using Dapper.CQRS;
using DotNetBackend.Data.DTO;
using System.Collections.Generic;

namespace DotNetBackend.Data.Commands
{
    public class CreatePlaylistVideoCommand : Command
    {
        private CreatePlaylistVideoDTO _dto;

        public CreatePlaylistVideoCommand(CreatePlaylistVideoDTO dto)
        {
            _dto = dto;
        }

        public override void Execute()
        {
            Execute(sql, _dto);
        }

        private string sql = @"
            INSERT INTO playlist_video (playlist_id, video_id)
            VALUES (@PlaylistId, @VideoId);
        ";
    }
}
