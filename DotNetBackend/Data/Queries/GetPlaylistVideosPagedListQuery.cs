using Dapper.CQRS;
using DotNetBackend.Data.DTO;
using DotNetBackend.Data.Requests;
using System;

namespace DotNetBackend.Data.Queries
{
    public class GetPlaylistVideosPagedListQuery : Query<PlaylistVideosDTO>
    {
        public string PlaylistId { get; set; }

        public GetPlaylistVideosPagedListQuery(GetPlaylistVideosRequest req)
        {
            PlaylistId = req.Id;
        }

        public override PlaylistVideosDTO Execute()
        {
            throw new NotImplementedException();
        }

        public string Sql => @"
            SELECT
                p.id,
                p.title,
                p.description,
                p.created_at,
                p.updated_at,
                p.last_checked_at
            FROM playlist AS p
            WHERE p.id = @Id;

            SELECT COUNT(pv.video_id)
            FROM playlist_video AS pv
            WHERE pv.playlist_id = @Id;

            SELECT DISTINCT
                v.id,
                v.title,
                v.channel_id,
                v.channel_name,
                v.channel_handle,
                v.duration
            FROM playlist_video AS pv
            INNER JOIN video AS v ON pv.video_id = v.id
            WHERE pv.playlist_id = @Id
            OFFSET @Offset
            LIMIT @PageSize;
        ";
    }
}
