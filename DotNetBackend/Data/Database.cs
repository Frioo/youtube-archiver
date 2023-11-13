using Npgsql;
using System.Threading.Tasks;

namespace DotNetBackend.Data
{
    public class Database
    {
        private static readonly string _connectionString = "Host=localhost;Username=yta;Password=yta;Database=YouTubeArchiver";
        private static readonly NpgsqlDataSource dataSource = new NpgsqlDataSourceBuilder(_connectionString).Build();

        public static NpgsqlConnection CreateDatabaseConnection()
        {
            return dataSource.OpenConnection();
        }
    }
}
