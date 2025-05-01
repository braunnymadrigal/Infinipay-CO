using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private SqlConnection _connection;
        private string? _pathConnection;

        public LoginController(IConfiguration config)
        {
            _config = config;
            var builder = WebApplication.CreateBuilder();
            _pathConnection = builder.Configuration.GetConnectionString("InfinipayDBContext");
            _connection = new SqlConnection(_pathConnection);
        }
    }
}
