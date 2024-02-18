using Dapper;
using Dapper.CQRS;
using DotNetBackend.Data.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetBackend.Data.Commands
{
    public class CreateVideoCommand : CommandAsync<string>
    {
        private CreateVideoDTO _dto;

        public CreateVideoCommand(CreateVideoDTO dto)
        {
            _dto = dto;
        }

        public override async Task<string> ExecuteAsync()
        {
            var thumbFilename = string.Concat(Guid.NewGuid(), Path.GetExtension(_dto.ThumbnailUri.LocalPath));
            var thumbPath = Path.Join(Environment.CurrentDirectory, "Files", thumbFilename);
            
            await new WebClient().DownloadFileTaskAsync(_dto.ThumbnailUri, thumbPath);

            var parameters = new DynamicParameters(_dto);
            parameters.Add("Thumbnail", thumbFilename);
            
            var res = await QueryFirstAsync<string>(Sql, parameters);

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
                thumbnail,
                channel_id,
                channel_name,
                channel_handle,
                duration,
                created_at
            )
            VALUES (
                @Id,
                @Title,
                @Description,
                @Thumbnail,
                @ChannelId,
                @ChannelName,
                @ChannelHandle,
                @Duration,
                @CreatedAt
            )
            ON CONFLICT (id) DO UPDATE
            SET
                id = @Id,
                title = @Title,
                description = @Description,
                thumbnail = @Thumbnail,
                channel_id = @ChannelId,
                channel_name = @ChannelName,
                channel_handle = @ChannelHandle,
                duration = @Duration,
                created_at = @CreatedAt
            RETURNING id;
        ";
    }
}
