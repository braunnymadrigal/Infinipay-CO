using back_end.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace back_end.Infraestructure
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

    public string getUsernameByPersonId(string personId)
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

    public bool createNewEmployee(EmployeeModel employee, string loggedId)
    {
      using (var connection = GetConnection())
      {
        connection.Open();
        using (var transaction = connection.BeginTransaction())
        {
          try
          {
            var cmd = new SqlCommand("sp_CreateNewEmployee", connection
              , transaction)
            {
              CommandType = CommandType.StoredProcedure
            };

            addAuditParameters(cmd, loggedId);
            addPersonParameters(cmd, employee);
            addNaturalPersonParameters(cmd, employee);
            addAddressParameters(cmd, employee);
            addUserParameters(cmd, employee);
            addEmployeeParameters(cmd, employee, loggedId);
            addContractParameters(cmd, employee);

            cmd.ExecuteNonQuery();
            transaction.Commit();
            return true;
          }
          catch (SqlException ex)
          {
            Debug.WriteLine("Stored procedure error: " + ex.Message);
            transaction.Rollback();
            throw;
          }
        }
      }
    }

    private void addAuditParameters(SqlCommand cmd, string loggedId)
    {
      string loggedUsername = getUsernameByPersonId(loggedId);
      cmd.Parameters.AddWithValue("@loggedPersonId", Guid.Parse(loggedId));

    }

    private void addPersonParameters(SqlCommand cmd, EmployeeModel employee)
    {
      cmd.Parameters.AddWithValue("@birthDay", employee.birthDay);
      cmd.Parameters.AddWithValue("@birthMonth", employee.birthMonth);
      cmd.Parameters.AddWithValue("@birthYear", employee.birthYear);
      cmd.Parameters.AddWithValue("@idNumber", employee.idNumber);
      cmd.Parameters.AddWithValue("@phoneNumber", employee.phoneNumber);
      cmd.Parameters.AddWithValue("@email", employee.email);

    }

    private void addNaturalPersonParameters(SqlCommand cmd
      , EmployeeModel employee)
    {
      cmd.Parameters.AddWithValue("@firstName", employee.firstName);
      cmd.Parameters.AddWithValue("@secondName"
        , (object?)employee.secondName ?? DBNull.Value);
      cmd.Parameters.AddWithValue("@firstLastName", employee.firstLastName);
      cmd.Parameters.AddWithValue("@secondLastName", employee.secondLastName);
      cmd.Parameters.AddWithValue("@gender", employee.gender);
    }

    private void addAddressParameters(SqlCommand cmd, EmployeeModel employee)
    {
      cmd.Parameters.AddWithValue("@province", employee.province);
      cmd.Parameters.AddWithValue("@canton", employee.canton);
      cmd.Parameters.AddWithValue("@district", employee.district);
      cmd.Parameters.AddWithValue("@otherSigns"
        , (object?)employee.otherSigns ?? DBNull.Value);
    }

    private void addUserParameters(SqlCommand cmd, EmployeeModel employee)
    {
      var birthDate = new DateTime(employee.birthYear, employee.birthMonth
        , employee.birthDay);
      var rawPassword = employee.firstLastName +
        birthDate.ToString("ddMMyyyy") + "!";
      cmd.Parameters.AddWithValue("@username", employee.username);
      cmd.Parameters.AddWithValue("@password", rawPassword);
    }

    private void addEmployeeParameters(SqlCommand cmd, EmployeeModel employee
      , string loggedId)
    {
      cmd.Parameters.AddWithValue("@hireDay", employee.hireDay);
      cmd.Parameters.AddWithValue("@hireMonth", employee.hireMonth);
      cmd.Parameters.AddWithValue("@hireYear", employee.hireYear);
      cmd.Parameters.AddWithValue("@role"
        , (object?)employee.role ?? DBNull.Value);

    }

    private void addContractParameters(SqlCommand cmd, EmployeeModel employee)
    {
      cmd.Parameters.AddWithValue("@creationDay", employee.creationDay);
      cmd.Parameters.AddWithValue("@creationMonth", employee.creationMonth);
      cmd.Parameters.AddWithValue("@creationYear", employee.creationYear);
      cmd.Parameters.AddWithValue("@reportsHours", employee.reportsHours);
      cmd.Parameters.AddWithValue("@salary", employee.salary);
      cmd.Parameters.AddWithValue("@typeContract", employee.typeContract);

    }
  }
}
