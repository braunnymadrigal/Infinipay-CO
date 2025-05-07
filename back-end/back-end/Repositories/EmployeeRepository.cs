using back_end.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace back_end.Repositories
{
  public class EmployeeRepository
  {
    private readonly string _connectionRoute;

    public EmployeeRepository()
    {
      var builder = WebApplication.CreateBuilder();
      _connectionRoute =
        builder.Configuration.GetConnectionString("InfinipayDBContext");
    }

    private SqlConnection GetConnection() =>
      new SqlConnection(_connectionRoute);

    private bool dataAlreadyExists(string table, string field, string value
      , SqlTransaction transaction)
    {
      var cmd = new SqlCommand(
          $"SELECT COUNT(*) FROM [{table}] WHERE [{field}] = @value",
          transaction.Connection, transaction);
      cmd.Parameters.AddWithValue("@value", value);
      var count = (int)cmd.ExecuteScalar();
      return count > 0;
    }
    public string GetUsernameByPersonId(string personId)
    {
      using (var connection = GetConnection())
      {
        connection.Open();
        var query =
          "SELECT nickname FROM Usuario WHERE idPersonaFisica = @personId";
        using (var cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@personId", personId);
          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              return reader.GetString(0);
            }
          }
        }
      }
      throw new Exception("No se encontró un usuario con ese ID.");
    }

    public bool createNewEmployee(EmployeeModel employee, string logguedId)
    {
      using (var connection = GetConnection())
      {
        connection.Open();
        using (var transaction = connection.BeginTransaction())
        {
          try
          {
            if (dataAlreadyExists("Persona", "identificacion"
              , employee.idNumber, transaction))
              throw new Exception("CEDULA_DUPLICADA");

            if (dataAlreadyExists("Persona", "numeroTelefono"
              , employee.phoneNumber, transaction))
              throw new Exception("TELEFONO_DUPLICADO");

            if (dataAlreadyExists("Persona", "correoElectronico"
              , employee.email, transaction))
              throw new Exception("EMAIL_DUPLICADO");

            if (dataAlreadyExists("Usuario", "nickname"
              , employee.username, transaction))
              throw new Exception("USERNAME_DUPLICADO");

            string loggedUsername = GetUsernameByPersonId(logguedId);
            var auditId = insertAudit(loggedUsername, transaction);
            var personId = insertPerson(employee, auditId, transaction);

            insertNaturalPerson(employee, personId, transaction);
            insertAddress(employee, personId, transaction);
            insertUser(employee, personId, transaction);
            insertEmployeeDetails(employee, logguedId, personId
              , transaction);
            insertEmployeeContractDetails(employee, personId, transaction);

            transaction.Commit();
            Debug.WriteLine("Successful insertion in all tables.");
            return true;
          }
          catch (Exception ex)
          {
            Debug.WriteLine("Insert error: " + ex.Message);
            transaction.Rollback();
            throw;
          }
        }
      }
    }

    private Guid insertAudit(string username, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                INSERT INTO [dbo].[Auditoria] ([usuarioCreador])
                OUTPUT INSERTED.id
                VALUES (@usuarioCreador)", transaction.Connection
                , transaction);
      cmd.Parameters.AddWithValue("@usuarioCreador", username);
      var auditId = (Guid)cmd.ExecuteScalar();
      Debug.WriteLine("Inserted audit with ID: " + auditId);
      return auditId;
    }

    private Guid insertPerson(EmployeeModel employee, Guid auditId
      , SqlTransaction transaction)
    {
      string idType = "fisica";

      var cmd = new SqlCommand(@"
                INSERT INTO [dbo].[Persona]
                ([identificacion], [numeroTelefono], [correoElectronico],
                [tipoIdentificacion], [idAuditoria], [fechaNacimiento])
                OUTPUT INSERTED.id
                VALUES (@identificacion, @numeroTelefono, @correoElectronico,
                @tipoIdentificacion, @idAuditoria, @fechaNacimiento)",
          transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@identificacion", employee.idNumber);
      cmd.Parameters.AddWithValue("@numeroTelefono", employee.phoneNumber);
      cmd.Parameters.AddWithValue("@correoElectronico", employee.email);
      cmd.Parameters.AddWithValue("@tipoIdentificacion", idType);
      cmd.Parameters.AddWithValue("@idAuditoria", auditId);
      var birthDate = new DateTime(employee.birthYear
        , employee.birthMonth, employee.birthDay);
      cmd.Parameters.Add("@fechaNacimiento"
        , SqlDbType.Date).Value = birthDate;

      var personId = (Guid)cmd.ExecuteScalar();

      return personId;
    }

    private void insertNaturalPerson(EmployeeModel employee, Guid personId
      , SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
          INSERT INTO [dbo].[PersonaFisica]
          ([id], [primerNombre], [segundoNombre], [primerApellido],
          [segundoApellido], [genero])
          VALUES (@id, @primerNombre, @segundoNombre, @primerApellido,
          @segundoApellido, @genero)",
          transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@id", personId);
      cmd.Parameters.AddWithValue("@primerNombre", employee.firstName);
      cmd.Parameters.AddWithValue("@segundoNombre",
        employee.secondName ?? (object)DBNull.Value);
      cmd.Parameters.AddWithValue("@primerApellido", employee.firstLastName);
      cmd.Parameters.AddWithValue("@segundoApellido",
        employee.secondLastName);
      cmd.Parameters.AddWithValue("@genero", employee.gender);
      Debug.WriteLine("NaturalPerson Done");
      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: insertNaturalPerson.");
    }

    private void insertAddress(EmployeeModel employee, Guid personId
      , SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                INSERT INTO [dbo].[Direccion]
                ([idPersona], [provincia], [canton], [distrito], [otrasSenas])
                VALUES (@idPersona, @provincia, @canton, @distrito
                , @otrasSenas)",
          transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@idPersona", personId);
      cmd.Parameters.AddWithValue("@provincia", employee.province);
      cmd.Parameters.AddWithValue("@canton", employee.canton);
      cmd.Parameters.AddWithValue("@distrito", employee.district);
      cmd.Parameters.AddWithValue("@otrasSenas"
        , employee.otherSigns ?? (object)DBNull.Value);
      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: insertAddress.");
    }
    private void insertUser(EmployeeModel employee, Guid personId
  , SqlTransaction transaction)
    {
      var birthDate = new DateTime(employee.birthYear, employee.birthMonth
        , employee.birthDay);
      var rawPassword = employee.firstLastName + birthDate.ToString("ddMMyyyy")
        + "!";
      var cmd = new SqlCommand(@"
        INSERT INTO [dbo].[Usuario]
        ([idPersonaFisica], [nickname], [contrasena])
        VALUES (@idPersonaFisica, @nickname,
        HASHBYTES('SHA2_512', CONVERT(varchar(100), @contrasena)))",
      transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@idPersonaFisica", personId);
      cmd.Parameters.AddWithValue("@nickname", employee.username);
      cmd.Parameters.AddWithValue("@contrasena", rawPassword);
      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: Usuario.");
    }

    private void insertEmployeeDetails(EmployeeModel employee
      , string logguedId, Guid personId, SqlTransaction transaction)
    {
      var hireDate = new DateTime(employee.hireYear, employee.hireMonth
        , employee.hireDay);

      var cmd = new SqlCommand(@"
                INSERT INTO [dbo].[Empleado]
                ([idPersonaFisica], [rol], [fechaContratacion ]
                , [idEmpleadorContratador])
                VALUES (@idPersonaFisica, @rol, @fechaContratacion
                , @idEmpleadorContratador)",
                transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@idPersonaFisica", personId);
      cmd.Parameters.AddWithValue("@rol"
         , employee.role ?? (object)DBNull.Value);
      cmd.Parameters.Add("@fechaContratacion"
        , SqlDbType.Date).Value = hireDate;
      cmd.Parameters.AddWithValue("@idEmpleadorContratador", logguedId);
      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: assignCompanyToEmployer.");
    }

    private void insertEmployeeContractDetails(EmployeeModel employee
  , Guid naturalPersonId, SqlTransaction transaction)
    {
      var creationDate = new DateTime(employee.creationYear
        , employee.creationMonth, employee.creationDay);

      var cmd = new SqlCommand(@"
                INSERT INTO [dbo].[Contrato]
                ([reportaHoras], [fechaCreacion], [salarioBruto]
                , [tipoContrato], [idEmpleado])
                VALUES (@reportaHoras, @fechaCreacion, @salarioBruto
                , @tipoContrato, @idEmpleado)",
                transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@reportaHoras", employee.reportsHours);
      cmd.Parameters.Add("@fechaCreacion"
        , SqlDbType.Date).Value = creationDate;
      cmd.Parameters.AddWithValue("@salarioBruto", employee.salary);
      cmd.Parameters.AddWithValue("@tipoContrato", employee.typeContract);
      cmd.Parameters.AddWithValue("@idEmpleado", naturalPersonId);
      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: assignCompanyToEmployer.");
    }
  }
}
