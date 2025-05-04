using back_end.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace back_end.Repositories
{
  public class EmployerRepository
  {
    private SqlConnection _connection;
    private string _connectionRoute;

    public EmployerRepository()
    {
      var builder = WebApplication.CreateBuilder();
      _connectionRoute =
        builder.Configuration.GetConnectionString("InfinipayDBContext");
      _connection = new SqlConnection(_connectionRoute);
    }

    private DataTable getQueryTable(string query)
    {
      var queryCommand = new SqlCommand(query, _connection);
      var tableAdapter = new SqlDataAdapter(queryCommand);
      var queryTable = new DataTable();
      _connection.Open();
      tableAdapter.Fill(queryTable);
      _connection.Close();
      return queryTable;
    }
    private bool dataAlreadyExists(string table, string field, string value
      , SqlTransaction tx)
    {
      var cmd = new SqlCommand(
        $"SELECT COUNT(*) FROM [{table}] WHERE [{field}] = @value"
        , _connection, tx);
      cmd.Parameters.AddWithValue("@value", value);
      var count = (int)cmd.ExecuteScalar();
      return count > 0;
    }

    public bool createNewEmployer(EmployerModel employer)
    {
      _connection.Open();
      var transaction = _connection.BeginTransaction();
      var isSuccess = false;

      try
      {

        if (dataAlreadyExists("Persona", "identificacion"
          , employer.idNumber, transaction))
          throw new Exception("CEDULA_DUPLICADA");

        if (dataAlreadyExists("Persona", "numeroTelefono", employer.phoneNumber
          , transaction))
          throw new Exception("TELEFONO_DUPLICADO");

        if (dataAlreadyExists("Persona", "correoElectronico", employer.email
          , transaction))
          throw new Exception("EMAIL_DUPLICADO");

        if (dataAlreadyExists("Usuario", "nickname", employer.username
          , transaction))
          throw new Exception("USERNAME_DUPLICADO");

        var auditId = insertAudit(employer.username, transaction);
        var personId = insertPerson(employer, auditId, transaction);
        insertNaturalPerson(employer, personId, transaction);
        insertAddress(employer, personId, transaction);
        insertUser(employer, personId, transaction);
        transaction.Commit();
        isSuccess = true;
        Debug.WriteLine("Successful insertion in all tables.");
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Insert error: " + ex.Message);
        transaction.Rollback();
        throw;
      }
      finally
      {
        _connection.Close();
      }

      return isSuccess;
    }

    private Guid insertAudit(string username, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
        INSERT INTO [dbo].[Auditoria] ([usuarioCreador])
        OUTPUT INSERTED.id
        VALUES (@usuarioCreador)", _connection, transaction);

      cmd.Parameters.AddWithValue("@usuarioCreador", username);
      var id = (Guid)cmd.ExecuteScalar();
      Debug.WriteLine("Inserted audit with ID: " + id);
      return id;
    }

    private Guid insertPerson(EmployerModel employer, Guid auditId,
                              SqlTransaction transaction)
    {
      string idType = "fisica";

      var cmd = new SqlCommand(@"
        INSERT INTO [dbo].[Persona]
        ([identificacion], [numeroTelefono], [correoElectronico],
         [tipoIdentificacion], [idAuditoria], [fechaNacimiento])
        OUTPUT INSERTED.id
        VALUES (@identificacion, @numeroTelefono, @correoElectronico,
         @tipoIdentificacion, @idAuditoria, @fechaNacimiento)"
        , _connection, transaction);

      cmd.Parameters.AddWithValue("@identificacion", employer.idNumber);
      cmd.Parameters.AddWithValue("@numeroTelefono", employer.phoneNumber);
      cmd.Parameters.AddWithValue("@correoElectronico", employer.email);
      cmd.Parameters.AddWithValue("@tipoIdentificacion", idType);
      cmd.Parameters.AddWithValue("@idAuditoria", auditId);

      var birthDate = new DateTime(employer.birthYear, employer.birthMonth,
                             employer.birthDay);
      cmd.Parameters.Add("@fechaNacimiento", SqlDbType.Date).Value =
        birthDate;

      var id = (Guid)cmd.ExecuteScalar();
      Debug.WriteLine("Inserted person with ID: " + id);
      return id;
    }

    private void insertNaturalPerson(EmployerModel employer, Guid personId,
                                     SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
        INSERT INTO [dbo].[PersonaFisica]
        ([id], [primerNombre], [segundoNombre], [primerApellido],
         [segundoApellido], [genero])
        VALUES (@id, @primerNombre, @segundoNombre, @primerApellido,
         @segundoApellido, @genero)", _connection
         , transaction);

      cmd.Parameters.AddWithValue("@id", personId);
      cmd.Parameters.AddWithValue("@primerNombre", employer.firstName);
      cmd.Parameters.AddWithValue("@segundoNombre",
        employer.secondName ?? (object)DBNull.Value);
      cmd.Parameters.AddWithValue("@primerApellido", employer.firstLastName);
      cmd.Parameters.AddWithValue("@segundoApellido",
        employer.secondLastName);
      cmd.Parameters.AddWithValue("@genero", employer.gender);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: PersonaFisica.");
    }

    private void insertAddress(EmployerModel employer, Guid personId,
                               SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
        INSERT INTO [dbo].[Direccion]
        ([idPersona], [provincia], [canton], [distrito], [otrasSenas])
        VALUES (@idPersona, @provincia, @canton, @distrito, @otrasSenas)",
        _connection, transaction);

      cmd.Parameters.AddWithValue("@idPersona", personId);
      cmd.Parameters.AddWithValue("@provincia", employer.province);
      cmd.Parameters.AddWithValue("@canton", employer.canton);
      cmd.Parameters.AddWithValue("@distrito", employer.district);
      cmd.Parameters.AddWithValue("@otrasSenas",
        employer.otherSigns ?? (object)DBNull.Value);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: Direccion.");
    }

    private void insertUser(EmployerModel employer, Guid personId
      , SqlTransaction transaction)
    {
      var birthDate = new DateTime(employer.birthYear, employer.birthMonth
        , employer.birthDay);
      var rawPassword = employer.firstLastName + birthDate.ToString("ddMMyyyy")
        + "!";
      Debug.WriteLine(rawPassword);
      var cmd = new SqlCommand(@"
        INSERT INTO [dbo].[Usuario]
        ([idPersonaFisica], [nickname], [contrasena])
        VALUES (@idPersonaFisica, @nickname,
        HASHBYTES('SHA2_512', CONVERT(varchar(100), @contrasena)))",
      _connection, transaction);
      cmd.Parameters.AddWithValue("@idPersonaFisica", personId);
      cmd.Parameters.AddWithValue("@nickname", employer.username);
      cmd.Parameters.AddWithValue("@contrasena", rawPassword);
      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: Usuario.");
    }
  }
}
