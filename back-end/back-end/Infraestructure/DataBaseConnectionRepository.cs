using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace back_end.Infraestructure
{
  public class DataBaseConnectionRepository
  {
    private readonly string _connectionString;

    public DataBaseConnectionRepository()
    {
      var builder = WebApplication.CreateBuilder();
      _connectionString =
        builder.Configuration.GetConnectionString("InfinipayDBContext");
    }

    public DataTable ExecuteQuery(string query)
    {
      using var connection = new SqlConnection(_connectionString);
      using var command = new SqlCommand(query, connection);
      var adapter = new SqlDataAdapter(command);
      var table = new DataTable();
      connection.Open();
      adapter.Fill(table);
      return table;
    }
  }
}
