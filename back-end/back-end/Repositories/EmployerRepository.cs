using back_end.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
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
    private DataTable createQueryTable(string query)
    {
      SqlCommand queryCommand = new SqlCommand(query, _connection);
      SqlDataAdapter tableAdapter = new SqlDataAdapter(queryCommand);
      DataTable queryTable = new DataTable();
      _connection.Open();
      tableAdapter.Fill(queryTable);
      _connection.Close();
      return queryTable;
    }
    public bool createNewEmployer(EmployerModel employer)
    {
      var consulta = 
      @"INSERT INTO [dbo].[Persona]
      ([identificacion], [numeroTelefono], [correoElectronico]
        , [tipoIdentificacion])
      OUTPUT INSERTED.id
      VALUES(@identificacion, @numeroTelefono, @correoElectronico
        , @tipoIdentificacion)";

      _connection.Open();
      SqlTransaction transaction = _connection.BeginTransaction();
      bool exito = false;

      try
      {
        Debug.WriteLine("Conectado a base de datos: " + _connection.Database);
        var personData = new SqlCommand(consulta, _connection, transaction);
        personData.Parameters.AddWithValue("@identificacion"
          , employer.idNumber);
        personData.Parameters.AddWithValue("@numeroTelefono"
          , employer.phoneNumber);
        personData.Parameters.AddWithValue("@correoElectronico"
          , employer.email);
        personData.Parameters.AddWithValue("@tipoIdentificacion"
          , employer.idType);

        Guid personaId = (Guid)personData.ExecuteScalar();
        Debug.WriteLine("Insertado Persona con ID: " + personaId);

        var naturalPersonQuery =
        @"INSERT INTO [dbo].[PersonaFisica]
        ([id], [primerNombre], [segundoNombre], [primerApellido]
          , [segundoApellido])
        VALUES(@id, @primerNombre, @segundoNombre, @primerApellido
          , @segundoApellido)";

        var naturalPersonData = new SqlCommand(naturalPersonQuery
          , _connection, transaction);
        naturalPersonData.Parameters.AddWithValue("@id", personaId);
        naturalPersonData.Parameters.AddWithValue("@primerNombre"
          , employer.firstName);
        naturalPersonData.Parameters.AddWithValue("@segundoNombre"
          , employer.secondName);
        naturalPersonData.Parameters.AddWithValue("@primerApellido"
          , employer.firstLastName);
        naturalPersonData.Parameters.AddWithValue("@segundoApellido"
          , employer.secondLastName);

        Debug.WriteLine("naturalPersonData.");
        if (naturalPersonData.ExecuteNonQuery() >= 1)
{
          var addressQuery =
          @"INSERT INTO [dbo].[Direccion]
          ([idPersona], [provincia], [canton], [distrito], [otrasSeñas])
          VALUES(@idPersona, @provincia, @canton, @distrito, @otrasSeñas)";

          var addressData = new SqlCommand(addressQuery, _connection
            , transaction);
          addressData.Parameters.AddWithValue("@idPersona", personaId);
          addressData.Parameters.AddWithValue("@provincia"
            , employer.province);
          addressData.Parameters.AddWithValue("@canton", employer.canton);
          addressData.Parameters.AddWithValue("@distrito", employer.district);
          addressData.Parameters.AddWithValue("@otrasSeñas"
            , employer.otherSigns ?? (object)DBNull.Value);
          
          Debug.WriteLine("addressData.");
          if (addressData.ExecuteNonQuery() >= 1)
          {
            transaction.Commit();
            exito = true;
            Debug.WriteLine("Insert exitoso en todas las tablas.");
          }
          else
          {
            Debug.WriteLine("Fallo al insertar en Direccion.");
            transaction.Rollback();
          }
        }
        else
        {
          Debug.WriteLine("Fallo al insertar en PersonaFisica.");
          transaction.Rollback();
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine("Error al insertar datos: " + ex.Message);
        transaction.Rollback();
      }
      finally
      {
        _connection.Close();
      }

      return exito;
    }
  }
}
