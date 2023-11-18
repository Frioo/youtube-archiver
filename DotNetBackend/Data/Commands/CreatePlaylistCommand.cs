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
            INSERT INTO playlist (id, title, description, created_at, updated_at, last_checked_at)
            VALUES (
                @Id,
                @Title,
                @Description,
                @CreatedAt,
                @UpdatedAt,
                @LastCheckedAt
            )
            ON CONFLICT (id) DO UPDATE
            SET
                title = @Title,
                description = @Description,
                updated_at = @UpdatedAt,
                last_checked_at = @LastCheckedAt;
        ";
    }
}
