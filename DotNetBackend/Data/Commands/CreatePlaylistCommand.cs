using Dapper.CQRS;
using DotNetBackend.Data.DTO;

namespace DotNetBackend.Data.Commands
{
    public class CreatePlaylistCommand : Command
    {
        private CreatePlaylistDTO _dto;

        public CreatePlaylistCommand(CreatePlaylistDTO dto)
        {
            _dto = dto;
        }

        public override void Execute()
        {
            var res = Execute(Sql, _dto);
        }

        private const string Sql = @"
            INSERT INTO playlist (id, title)
            VALUES (
                @Id,
                @Title
            )
            ON CONFLICT (id) DO NOTHING;
        ";
    }
}
