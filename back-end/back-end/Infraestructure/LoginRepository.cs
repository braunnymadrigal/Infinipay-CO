using System.Data;
using back_end.Domain;

using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
    public class LoginRepository : GeneralRepository
    {
        public LoginRepository() : base() { }

        public UserModel GetUserByNickname(string nickname)
        {
            var command = CreateUserByNicknameCommand(nickname);
            var dataTable = ExecuteQuery(command);
            var user = TransformDataTableIntoUserModel(dataTable);
            return user;
        }

        public UserModel GetUserByEmail(string email)
        {
            var command = CreateUserByEmailCommand(email);
            var dataTable = ExecuteQuery(command);
            var user = TransformDataTableIntoUserModel(dataTable);
            return user;
        }

        public void UpdateExactBlockDateInUser(string personId)
        {
            var statement = "UPDATE [Usuario] SET [fechaExactaBloqueo] = SYSDATETIME() WHERE [idPersonaFisica] = @personaId;";
            var command = new SqlCommand(statement, connection);
            command.Parameters.AddWithValue("@personaId", personId);
            ExecuteCommand(command);
        }

        public void UpdateNumAttemptsInUser(string personId, int numAttempts)
        {
            var statement = "UPDATE [Usuario] SET [numIntentos] = @numAttempts WHERE [idPersonaFisica] = @personaId;";
            var command = new SqlCommand(statement, connection);
            command.Parameters.AddWithValue("@personaId", personId);
            command.Parameters.AddWithValue("@numAttempts", numAttempts);
            ExecuteCommand(command);
        }

        private UserModel TransformDataTableIntoUserModel(DataTable dataTable)
        {
            if (dataTable.Rows.Count <= 0)
            {
                throw new Exception("User not found.");
            }
            var dataRow = dataTable.Rows[0];
            var personId = Convert.ToString(dataRow["personaId"]);
            var nickname = Convert.ToString(dataRow["usuarioNickname"]);
            var password = (byte[])(dataRow["usuarioContrasena"]);
            var numAttempts = Convert.ToString(dataRow["usuarioNumIntentos"]);
            var lastBlock = Convert.ToString(dataRow["usuarioFechaExactaBloqueo"]);
            var empleadoId = Convert.ToString(dataRow["empleadoId"]);
            var role = Convert.ToString(dataRow["empleadoRol"]);
            var empleadorId = Convert.ToString(dataRow["empleadorId"]);

            if (personId == null || nickname == null || password == null
                || numAttempts == null || lastBlock == null || empleadoId == null 
                || role == null || empleadorId == null)
            {
                throw new Exception("User not found or uncomplete.");
            }
            var userModel = new UserModel();
            userModel.Nickname = nickname;
            userModel.Password = password;
            userModel.PersonId = personId;
            userModel.Role = role;
            if (numAttempts != "")
            {
                userModel.NumAttempts = Int32.Parse(numAttempts);
            }
            if (lastBlock != "")
            {
                userModel.LastBlock = DateTime.Parse(lastBlock);
            }
            if (role == "")
            {
                if (empleadoId == "")
                {
                    if (empleadorId == "")
                    {
                        userModel.Role = "superAdmin";
                    }
                    else
                    {
                        userModel.Role = "empleador";
                    }
                }
                else
                {
                    userModel.Role = "empleado";
                }
            }
            return userModel;
        }

        private SqlCommand CreateUserByNicknameCommand(string nickname)
        {
            var query = CreateUserByNicknameQuery();
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@nickname", nickname);
            return command;
        }

        private SqlCommand CreateUserByEmailCommand(string email)
        {
            var query = CreateUserByEmailQuery();
            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@email", email);
            return command;
        }

        private string CreateUserByNicknameQuery()
        {
            var query = "SELECT p.id as personaId, "
	                    + "u.nickname as usuarioNickname, "
	                    + "u.contrasena as usuarioContrasena, "
	                    + "u.numIntentos as usuarioNumIntentos, "
	                    + "u.fechaExactaBloqueo as usuarioFechaExactaBloqueo, "
	                    + "e.idPersonaFisica as empleadoId, "
	                    + "e.rol as empleadoRol,"
	                    + "o.idPersonaFisica as empleadorId "
                        + "FROM [Persona] p "
                        + "FULL OUTER JOIN [Usuario] u on u.idPersonaFisica = p.id "
                        + "FULL OUTER JOIN [Empleado] e on e.idPersonaFisica = p.id "
                        + "FULL OUTER JOIN [Empleador] o on o.idPersonaFisica = p.id "
                        + "WHERE u.nickname = @nickname;";
            return query;
        }

        private string CreateUserByEmailQuery()
        {
            var query = "SELECT p.id as personaId, "
                        + "u.nickname as usuarioNickname, "
                        + "u.contrasena as usuarioContrasena, "
                        + "u.numIntentos as usuarioNumIntentos, "
                        + "u.fechaExactaBloqueo as usuarioFechaExactaBloqueo, "
                        + "e.idPersonaFisica as empleadoId, "
                        + "e.rol as empleadoRol, "
                        + "o.idPersonaFisica as empleadorId "
                        + "FROM [Persona] p "
                        + "FULL OUTER JOIN [Usuario] u on u.idPersonaFisica = p.id "
                        + "FULL OUTER JOIN [Empleado] e on e.idPersonaFisica = p.id "
                        + "FULL OUTER JOIN [Empleador] o on o.idPersonaFisica = p.id "
                        + "WHERE p.correoElectronico = @email; ";
            return query;
        }
    }
}
