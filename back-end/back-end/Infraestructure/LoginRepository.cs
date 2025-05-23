using back_end.Domain;

using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Infraestructure
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

        public string Login(LoginUserModel loginUserModel)
        {
            UserModel userModel = Authenticate(loginUserModel);
            return ( Generate(userModel) ); 
        }

        private string Generate(UserModel userModel)
        {
            var jwtConfigKey = _config["JwtConfig:Key"];
            if (jwtConfigKey == null)
            {
                throw new Exception("'JwtConfig:Key' IS MISSING");
            }
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
                if (userModel.Nickname == "")
                {
                    throw new Exception("USER NOT FOUND BY EMAIL");
                }
                consulta = $"SELECT * FROM [Usuario] WHERE [idPersonaFisica] = '{userModel.PersonaId}';";
            }
            else
            {
                consulta = $"SELECT * FROM [Usuario] WHERE [nickname]='{nicknameOrEmail}';";
            }
            userModel = ObtenerTablaUsuario(userModel, consulta);
            if (userModel.Nickname == "" || userModel.Password == null)
            {
                throw new Exception("USER NOT FOUND BY NICKNAME");
            }
            bool isItPossibleToLogin = IsItPossibleToLogin(userModel);
            if (!isItPossibleToLogin)
            {
                throw new Exception("LOGIN IS FORBIDDEN TO YOU");
            }
            okPassword = password.SequenceEqual(userModel.Password);
            if (!okPassword)
            {
                userModel = DoInCaseNotOkPassword(userModel);
                throw new Exception("WRONG PASSWORD");
            }
            userModel = DoInCaseOkPassword(userModel);
            return userModel;
        }

        private UserModel DoInCaseOkPassword(UserModel userModel)
        {
            string query = $"SELECT [rol] FROM [Empleado] WHERE [idPersonaFisica]='{userModel.PersonaId}';";
            userModel = ObtenerRol(userModel, query);
            query = $"UPDATE [Usuario] SET [numIntentos] = 0 WHERE [idPersonaFisica] = '{userModel.PersonaId}';";
            MakeAnUpdate(query);
            return userModel;
        }

        private UserModel DoInCaseNotOkPassword(UserModel userModel)
        {
            userModel.Nickname = "";
            int numAttempts = userModel.NumAttempts + 1;
            string query = $"UPDATE [Usuario] SET [numIntentos] = {numAttempts} WHERE [idPersonaFisica] = '{userModel.PersonaId}';";
            MakeAnUpdate(query);
            if (numAttempts >= 5)
            {
                query = $"UPDATE [Usuario] SET [fechaExactaBloqueo] = SYSDATETIME() WHERE [idPersonaFisica] = '{userModel.PersonaId}';";
                MakeAnUpdate(query);
            }
            return userModel;
        }

        private bool IsItPossibleToLogin(UserModel userModel)
        {
            bool isPossible = true;
            if (userModel.NumAttempts >= 5)
            {
                if (userModel.LastBlock == DateTime.MinValue)
                {
                    string query = $"UPDATE [Usuario] SET [fechaExactaBloqueo] = SYSDATETIME() WHERE [idPersonaFisica] = '{userModel.PersonaId}';";
                    MakeAnUpdate(query);
                    isPossible = false;
                }
                else
                {
                    DateTime currentDateTime = DateTime.Now;
                    DateTime lastBlockPlusTen = (userModel.LastBlock).AddMinutes(10);
                    int resultOfDateTimeComparison = DateTime.Compare(lastBlockPlusTen, currentDateTime);
                    if (resultOfDateTimeComparison >= 0)
                    {
                        isPossible = false;
                    }
                }
            }
            return isPossible;
        }

        private void MakeAnUpdate(string query)
        {
            SqlCommand command = new SqlCommand(query, _connection);
            _connection.Open();
            bool success = command.ExecuteNonQuery() >= 1;
            _connection.Close();
            if (!success)
            {
                throw new Exception("AN SQL UPDATE WAS NOT POSSIBLE");
            }
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
                var numAttempts = Convert.ToString(filaResultado["numIntentos"]);
                var lastBlock = Convert.ToString(filaResultado["fechaExactaBloqueo"]);
                if (nickname != null && password != null && personaId != null 
                    && numAttempts != null && lastBlock != null)
                {
                    userModel.Nickname = nickname;
                    userModel.Password = password;
                    userModel.PersonaId = personaId;
                    if (numAttempts != "")
                    {
                        userModel.NumAttempts= Int32.Parse(numAttempts);
                    }
                    if (lastBlock != "")
                    {
                        userModel.LastBlock = DateTime.Parse(lastBlock);
                    }
                    
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
