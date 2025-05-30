using back_end.Domain;

using System.Data;
using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AbstractConnectionRepository connectionRepository;
        private readonly IUtilityRepository utilityRepository;

        public LoginRepository()
        {
            connectionRepository = new ConnectionRepository();
            utilityRepository = new UtilityRepository();
        }

        public UserModel GetUserByNickname(string nickname)
        {
            var command = CreateUserByNicknameCommand(nickname);
            var dataTable = connectionRepository.ExecuteQuery(command);
            var user = TransformDataTableIntoUserModel(dataTable);
            return user;
        }

        public UserModel GetUserByEmail(string email)
        {
            var command = CreateUserByEmailCommand(email);
            var dataTable = connectionRepository.ExecuteQuery(command);
            var user = TransformDataTableIntoUserModel(dataTable);
            return user;
        }

        public void UpdateExactBlockDateInUser(string personId)
        {
            var statement = "UPDATE [Usuario] SET [fechaExactaBloqueo] = SYSDATETIME() WHERE [idPersonaFisica] = @personaId;";
            var command = new SqlCommand(statement, connectionRepository.connection);
            command.Parameters.AddWithValue("@personaId", personId);
            connectionRepository.ExecuteCommand(command);
        }

        public void UpdateNumAttemptsInUser(string personId, int numAttempts)
        {
            var statement = "UPDATE [Usuario] SET [numIntentos] = @numAttempts WHERE [idPersonaFisica] = @personaId;";
            var command = new SqlCommand(statement, connectionRepository.connection);
            command.Parameters.AddWithValue("@personaId", personId);
            command.Parameters.AddWithValue("@numAttempts", numAttempts);
            connectionRepository.ExecuteCommand(command);
        }

        private UserModel TransformDataTableIntoUserModel(DataTable dataTable)
        {
            if (dataTable.Rows.Count <= 0)
            {
                throw new Exception("User not found.");
            }
            var dataRow = dataTable.Rows[0];
            var userModel = new UserModel();
            userModel.PersonId = utilityRepository.ConvertDatabaseValueToString(dataRow["personaId"]);
            userModel.Nickname = utilityRepository.ConvertDatabaseValueToString(dataRow["usuarioNickname"]);
            userModel.Password = (byte[])(dataRow["usuarioContrasena"]);
            userModel.Role = utilityRepository.ConvertDatabaseValueToString(dataRow["empleadoRol"]);
            var numAttempts = utilityRepository.ConvertDatabaseValueToString(dataRow["usuarioNumIntentos"]);
            var lastBlock = utilityRepository.ConvertDatabaseValueToString(dataRow["usuarioFechaExactaBloqueo"]);
            var empleadoId = utilityRepository.ConvertDatabaseValueToString(dataRow["empleadoId"]);
            var empleadorId = utilityRepository.ConvertDatabaseValueToString(dataRow["empleadorId"]);
            userModel = SaveNumAttemptsToUserModel(numAttempts, userModel);
            userModel = SaveLastBlockToUserModel(lastBlock, userModel);
            userModel = SaveRoleToUserModel(userModel, empleadoId, empleadorId);
            return userModel;
        }

        private UserModel SaveRoleToUserModel(UserModel userModel, string empleadoId, string empleadorId)
        {
            if (userModel.Role == "")
            {
                userModel.Role = "empleado";
                if (empleadoId == "")
                {
                    userModel.Role = empleadorId == "" ? "superAdmin" : "empleador";
                }
            }
            return userModel;
        }

        private UserModel SaveNumAttemptsToUserModel(string numAttempts, UserModel userModel)
        {
            if (numAttempts != "")
            {
                userModel.NumAttempts = Int32.Parse(numAttempts);
            }
            return userModel;
        }

        private UserModel SaveLastBlockToUserModel (string  lastBlock, UserModel userModel)
        {
            if (lastBlock != "")
            {
                userModel.LastBlock = DateTime.Parse(lastBlock);
            }
            return userModel;
        }

        private SqlCommand CreateUserByNicknameCommand(string nickname)
        {
            var query = CreateUserByNicknameQuery();
            var command = new SqlCommand(query, connectionRepository.connection);
            command.Parameters.AddWithValue("@nickname", nickname);
            return command;
        }

        private SqlCommand CreateUserByEmailCommand(string email)
        {
            var query = CreateUserByEmailQuery();
            var command = new SqlCommand(query, connectionRepository.connection);
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
