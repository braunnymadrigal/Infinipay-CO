using back_end.Models;

using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

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

        [AllowAnonymous]
        [Route("userLogin")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginUserModel loginUserModel)
        {
            UserModel userModel = Authenticate(loginUserModel);
            if (userModel.Nickname != "")
            {
                String token = Generate(userModel);
                return Ok(token);
            }
            return NotFound("User not found");
        }

        private string Generate(UserModel userModel)
        {
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
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return "error";
            }
        }

        private UserModel Authenticate(LoginUserModel loginUserModel)
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
                consulta = $"SELECT [nickname], [contrasena], [idPersonaFisica] FROM [Usuario] WHERE [idPersonaFisica]='{userModel.PersonaId}';";
            }
            else
            {
                consulta = $"SELECT [nickname], [contrasena], [idPersonaFisica] FROM [Usuario] WHERE [nickname]='{nicknameOrEmail}';";
            }
            userModel = ObtenerTablaUsuario(userModel, consulta);
            if (userModel.Nickname != "" && userModel.Password != null)
            {
                okPassword = password.SequenceEqual(userModel.Password);
            }
            if (!okPassword)
            {
                userModel.Nickname = "";
            } else
            {
                consulta = $"SELECT [rol] FROM [Empleado] WHERE [idPersonaFisica]='{userModel.PersonaId}';";
                userModel = ObtenerRol(userModel, consulta);
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
                var personaId = Convert.ToString(filaResultado["id"]);
                if (email != null && personaId != null)
                {
                    userModel.Nickname = email;
                    userModel.PersonaId = personaId;
                }
            }
            return userModel;
        }

        [Route("GetUser")]
        [HttpPost]
        public UserModel GetCurrentUser()
        {
            UserModel userModel = new UserModel();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;

                var Nickname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                var PersonaId = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value;
                var Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;

                if (Nickname != null && PersonaId != null && Role != null)
                {
                    userModel.Nickname = Nickname;
                    userModel.PersonaId = PersonaId;
                    userModel.Role = Role;
                }
            }
            return userModel;
        }
    }
}
