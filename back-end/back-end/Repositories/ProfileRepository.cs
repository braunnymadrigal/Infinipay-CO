using back_end.Models;

using Microsoft.Data.SqlClient;

namespace back_end.Repositories
{
    public class ProfileRepository
    {
        private readonly SqlConnection _connection;
        private readonly string? _pathConnection;

        public ProfileRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _pathConnection = builder.Configuration.GetConnectionString("InfinipayDBContext");
            _connection = new SqlConnection(_pathConnection);
        }
    }
}
