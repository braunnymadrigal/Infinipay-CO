using System.Data;
using back_end.Models;
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

        private UserModel Authenticate(LoginUserModel loginUserModel)
        {
            string consulta = $"SELECT * FROM Usuario WHERE nickname='{loginUserModel.NicknameOrEmail}'";
            UserModel userModel = ObtenerUsuarioModelo(consulta);
            bool isUserOnDB = loginUserModel.NicknameOrEmail == userModel.Nickname && loginUserModel.Password == userModel.Password;
            if (!isUserOnDB)
            {
                userModel.Nickname = "";
                userModel.Password = "";
            }
            return userModel;
        }

        private DataTable CrearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _connection);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            _connection.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            _connection.Close();
            return consultaFormatoTabla;
        }

        private UserModel ObtenerUsuarioModelo(string consulta)
        {
            UserModel userModel = new UserModel { Nickname="" };    
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                DataRow filaResultado = tablaResultado.Rows[0];
                userModel.Nickname = Convert.ToString(filaResultado["nickname"]);
                userModel.Password = Convert.ToString(filaResultado["contraseña"]);
            }
            return userModel;
        }
    }
}
