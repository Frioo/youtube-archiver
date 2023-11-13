using Dapper.CQRS;
using DotNetBackend.Models;
using System.Collections.Generic;
using System.Linq;

namespace DotNetBackend.Data.Queries
{
    public class GetPlaylistPagedListQuery : Query<List<Playlist>>
    {
        public override List<Playlist> Execute()
        {
            var sql = @"
                SELECT
                    p.id,
                    p.title,
                    COUNT(pv) AS video_count
                FROM playlist AS p
                INNER JOIN playlist_video AS pv ON pv.playlist_id = p.id;
            ";

            var res = QueryList<Playlist>(sql);

            return res.ToList();
        }
    }
}
