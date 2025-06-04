using back_end.Domain;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Data.Common;

namespace back_end.Infraestructure
{
  public class EmployerRepository
  {
    private SqlConnection connection;
    private string connectionRoute;

    public EmployerRepository()
    {
      var builder = WebApplication.CreateBuilder();
      connectionRoute =
        builder.Configuration.GetConnectionString("InfinipayDBContext");
      connection = new SqlConnection(connectionRoute);
    }

    public bool createNewEmployer(EmployerModel employer)
    {
      connection.Open();
      var isSuccess = false;

      try
      {
        using (var cmd = new SqlCommand("sp_CreateNewEmployer", connection))
        {
          cmd.CommandType = CommandType.StoredProcedure;

          AddIdentificationInfo(cmd, employer);
          AddNameInfo(cmd, employer);
          AddAddressInfo(cmd, employer);
          AddUserInfo(cmd, employer);

          cmd.ExecuteNonQuery();
        }

        isSuccess = true;
      }
      catch (SqlException ex)
      {
        throw new Exception("No se pudo crear nuevo empleado " + ex.Message);
      }
      finally
      {
        connection.Close();
      }

      return isSuccess;
    }

    private void AddIdentificationInfo(SqlCommand cmd, EmployerModel employer)
    {
      cmd.Parameters.AddWithValue("@idNumber", employer.idNumber);
      cmd.Parameters.AddWithValue("@phoneNumber", employer.phoneNumber);
      cmd.Parameters.AddWithValue("@email", employer.email);
      cmd.Parameters.AddWithValue("@birthDay", employer.birthDay);
      cmd.Parameters.AddWithValue("@birthMonth", employer.birthMonth);
      cmd.Parameters.AddWithValue("@birthYear", employer.birthYear);
    }

    private void AddNameInfo(SqlCommand cmd, EmployerModel employer)
    {
      cmd.Parameters.AddWithValue("@firstName", employer.firstName);
      cmd.Parameters.AddWithValue("@secondName"
        , (object?)employer.secondName ?? DBNull.Value);
      cmd.Parameters.AddWithValue("@firstLastName"
        , employer.firstLastName);
      cmd.Parameters.AddWithValue("@secondLastName", employer.secondLastName);
      cmd.Parameters.AddWithValue("@gender", employer.gender);
    }

    private void AddAddressInfo(SqlCommand cmd, EmployerModel employer)
    {
      cmd.Parameters.AddWithValue("@province", employer.province);
      cmd.Parameters.AddWithValue("@canton", employer.canton);
      cmd.Parameters.AddWithValue("@district", employer.district);
      cmd.Parameters.AddWithValue("@otherSigns"
        , (object?)employer.otherSigns ?? DBNull.Value);
    }

    private void AddUserInfo(SqlCommand cmd, EmployerModel employer)
    {
      cmd.Parameters.AddWithValue("@username", employer.username);
      cmd.Parameters.AddWithValue("@password", employer.password);
    }
  }
}