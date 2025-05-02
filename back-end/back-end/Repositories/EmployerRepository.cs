using back_end.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace back_end.Handlers
{
  public class EmployerRepository
  {
    private SqlConnection _connection;
    private string _connectionRoute;

    public EmployerRepository()
    {
      var builder = WebApplication.CreateBuilder();
      _connectionRoute =
        builder.Configuration.GetConnectionString("InfinipayDB");
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
          , employer.idNumber.ToString(), transaction))
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

    private Guid insertPerson(EmployerModel emp, Guid auditId,
                              SqlTransaction tx)
    {
      var cmd = new SqlCommand(@"
        INSERT INTO [dbo].[Persona]
        ([identificacion], [numeroTelefono], [correoElectronico],
         [tipoIdentificacion], [idAuditoria])
        OUTPUT INSERTED.id
        VALUES (@identificacion, @numeroTelefono, @correoElectronico,
         @tipoIdentificacion, @idAuditoria)", _connection, tx);

      cmd.Parameters.AddWithValue("@identificacion", emp.idNumber);
      cmd.Parameters.AddWithValue("@numeroTelefono", emp.phoneNumber);
      cmd.Parameters.AddWithValue("@correoElectronico", emp.email);
      cmd.Parameters.AddWithValue("@tipoIdentificacion", emp.idType);
      cmd.Parameters.AddWithValue("@idAuditoria", auditId);

      var id = (Guid)cmd.ExecuteScalar();
      Debug.WriteLine("Inserted person with ID: " + id);
      return id;
    }

    private void insertNaturalPerson(EmployerModel emp, Guid personId,
                                     SqlTransaction tx)
    {
      var cmd = new SqlCommand(@"
        INSERT INTO [dbo].[PersonaFisica]
        ([id], [primerNombre], [segundoNombre], [primerApellido],
         [segundoApellido], [genero], [fechaNacimiento])
        VALUES (@id, @primerNombre, @segundoNombre, @primerApellido,
         @segundoApellido, @genero, @fechaNacimiento)", _connection, tx);

      cmd.Parameters.AddWithValue("@id", personId);
      cmd.Parameters.AddWithValue("@primerNombre", emp.firstName);
      cmd.Parameters.AddWithValue("@segundoNombre",
        emp.secondName ?? (object)DBNull.Value);
      cmd.Parameters.AddWithValue("@primerApellido", emp.firstLastName);
      cmd.Parameters.AddWithValue("@segundoApellido",
        emp.secondLastName);
      cmd.Parameters.AddWithValue("@genero", emp.gender);

      var birthDate = new DateTime(emp.birthYear, emp.birthMonth,
                                   emp.birthDay);
      cmd.Parameters.Add("@fechaNacimiento", SqlDbType.Date).Value =
        birthDate;

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: PersonaFisica.");
    }

    private void insertAddress(EmployerModel emp, Guid personId,
                               SqlTransaction tx)
    {
      var cmd = new SqlCommand(@"
        INSERT INTO [dbo].[Direccion]
        ([idPersona], [provincia], [canton], [distrito], [otrasSenas])
        VALUES (@idPersona, @provincia, @canton, @distrito, @otrasSenas)",
        _connection, tx);

      cmd.Parameters.AddWithValue("@idPersona", personId);
      cmd.Parameters.AddWithValue("@provincia", emp.province);
      cmd.Parameters.AddWithValue("@canton", emp.canton);
      cmd.Parameters.AddWithValue("@distrito", emp.district);
      cmd.Parameters.AddWithValue("@otrasSenas",
        emp.otherSigns ?? (object)DBNull.Value);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: Direccion.");
    }

    private void insertUser(EmployerModel emp, Guid personId,
                            SqlTransaction tx)
    {
      var birthDate = new DateTime(emp.birthYear, emp.birthMonth,
                                   emp.birthDay);
      var rawPassword = emp.firstLastName + birthDate.ToString("ddMMyyyy");
      var passwordBytes = Encoding.UTF8.GetBytes(rawPassword);
      Array.Resize(ref passwordBytes, 64);

      var cmd = new SqlCommand(@"
        INSERT INTO [dbo].[Usuario]
        ([idPersonaFisica], [nickname], [contrasena])
        VALUES (@idPersonaFisica, @nickname, @contrasena)",
        _connection, tx);

      cmd.Parameters.AddWithValue("@idPersonaFisica", personId);
      cmd.Parameters.AddWithValue("@nickname", emp.username);
      cmd.Parameters.AddWithValue("@contrasena", passwordBytes);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: Usuario.");
    }
  }
}
