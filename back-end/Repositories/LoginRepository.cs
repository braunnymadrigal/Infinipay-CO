using back_end.Models;

using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace back_end.Repositories
{
    public class LoginRepository
    {
        private readonly IConfiguration _config;
        private readonly SqlConnection _connection;
        private readonly string? _pathConnection;

        public LoginRepository(IConfiguration config)
        {
            var builder = WebApplication.CreateBuilder();
            _pathConnection = builder.Configuration.GetConnectionString("InfinipayDBContext");
            _connection = new SqlConnection(_pathConnection);
            _config = config;
        }

        public string Generate(UserModel userModel)
        {
            string returnString = "";
            var jwtConfigKey = _config["JwtConfig:Key"];
            if (jwtConfigKey != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userModel.Nickname),
                new Claim(ClaimTypes.Sid, userModel.PersonaId),
                new Claim(ClaimTypes.Role, userModel.Role),
            };
                var token = new JwtSecurityToken
                (
                    _config["JwtConfig:Issuer"],
                    _config["JwtConfig:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credentials
                );
                returnString = new JwtSecurityTokenHandler().WriteToken(token);
            }
            return returnString;
        }

        public UserModel Authenticate(LoginUserModel loginUserModel)
        {
            UserModel userModel = new UserModel();
            var nicknameOrEmail = (loginUserModel.NicknameOrEmail).ToLower();
            var inputBytes = Encoding.UTF8.GetBytes(loginUserModel.Password);
            var password = SHA512.HashData(inputBytes);
            string consulta = "";
            bool okPassword = false;
            if (nicknameOrEmail.Contains("@"))
            {
                consulta = $"SELECT [id], [correoElectronico] FROM [Persona] WHERE [correoElectronico] = '{nicknameOrEmail}';";
                userModel = ObtenerTablaPersona(userModel, consulta);
                if (userModel.Nickname != "")
                {
                    consulta = $"SELECT [nickname], [contrasena], [idPersonaFisica] FROM [Usuario] WHERE [idPersonaFisica] = '{userModel.PersonaId}';";
                    userModel = ObtenerTablaUsuario(userModel, consulta);
                }
            }
            else
            {
                consulta = $"SELECT [nickname], [contrasena], [idPersonaFisica] FROM [Usuario] WHERE [nickname]='{nicknameOrEmail}';";
                userModel = ObtenerTablaUsuario(userModel, consulta);
            }
            if (userModel.Nickname != "" && userModel.Password != null)
            {
                okPassword = password.SequenceEqual(userModel.Password);
            }
            if (!okPassword)
            {
                userModel.Nickname = "";
            }
            else
            {
                consulta = $"SELECT [rol] FROM [Empleado] WHERE [idPersonaFisica]='{userModel.PersonaId}';";
                userModel = ObtenerRol(userModel, consulta);
            }
            return userModel;
        }

        private UserModel ObtenerRol(UserModel userModel, string consulta)
        {
            userModel.Role = "empleador";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                userModel.Role = "empleado";
                DataRow filaResultado = tablaResultado.Rows[0];
                var tableRole = Convert.ToString(filaResultado["rol"]);
                if (tableRole != null && tableRole != "")
                {
                    userModel.Role = tableRole;
                }
            }
            return userModel;
        }

        private UserModel ObtenerTablaUsuario(UserModel userModel, string consulta)
        {
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                DataRow filaResultado = tablaResultado.Rows[0];
                var nickname = Convert.ToString(filaResultado["nickname"]);
                var password = (byte[])(filaResultado["contrasena"]);
                var personaId = Convert.ToString(filaResultado["idPersonaFisica"]);
                if (nickname != null && password != null && personaId != null)
                {
                    userModel.Nickname = nickname;
                    userModel.Password = password;
                    userModel.PersonaId = personaId;
                }
            }
            return userModel;
        }

        private UserModel ObtenerTablaPersona(UserModel userModel, string consulta)
        {
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                DataRow filaResultado = tablaResultado.Rows[0];
                var email = Convert.ToString(filaResultado["correoElectronico"]);
                var id = Convert.ToString(filaResultado["id"]);
                if (email != null && id != null)
                {
                    userModel.Nickname = email;
                    userModel.PersonaId = id;
                }
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
    }
}
