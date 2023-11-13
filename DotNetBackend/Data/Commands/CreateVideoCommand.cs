using Dapper.CQRS;
using DotNetBackend.Data.DTO;
using System.Collections.Generic;

namespace DotNetBackend.Data.Commands
{
    public class CreateVideoCommand : Command<string>
    {
        private CreateVideoDTO _dto;

        public CreateVideoCommand(CreateVideoDTO dto)
        {
            _dto = dto;
        }

        public override string Execute()
        {
            var res = QueryFirst<string>(Sql, _dto);

            return res;
        }

        private const string Sql = @"
            INSERT INTO channel (
                    id,
                    name,
                    handle
                )
            VALUES (
                @ChannelId,
                @ChannelName,
                @ChannelHandle
            )
            ON CONFLICT (id) DO NOTHING;

            INSERT INTO video (
                id,
                title,
                description,
                channel_id,
                channel_name,
                channel_handle,
                duration
            )
            VALUES (
                @Id,
                @Title,
                @Description,
                @ChannelId,
                @ChannelName,
                @ChannelHandle,
                @Duration
            )
            ON CONFLICT (id) DO UPDATE
            SET
                id = @Id,
                title = @Title,
                description = @Description,
                channel_id = @ChannelId,
                channel_name = @ChannelName,
                channel_handle = @ChannelHandle,
                duration = @Duration
            RETURNING id;
        ";
    }
}
