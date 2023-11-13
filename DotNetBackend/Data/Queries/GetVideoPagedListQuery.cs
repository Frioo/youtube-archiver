using Dapper.CQRS;
using DotNetBackend.Models;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBackend.Data.Queries
{
    public class GetVideoPagedListQuery : Query<List<Video>>
    {
        public override List<Video> Execute()
        {
            var sql = @$"
                SELECT
                    v.id,
                    v.title,
                    v.description,
                    v.channel_id,
                    v.channel_name,
                    v.channel_handle,
                    v.duration
                FROM video AS v;
            ";
            var videos = QueryList<Video>(sql);

            return videos.ToList();
        }
    }
}
