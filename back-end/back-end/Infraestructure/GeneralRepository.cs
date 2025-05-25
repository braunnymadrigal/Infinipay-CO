using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace back_end.Infraestructure
{
    public abstract class GeneralRepository
    {
        private readonly SqlConnection connection;
        private readonly string? connectionString;

        protected GeneralRepository()
        {
            var builder = WebApplication.CreateBuilder();
            connectionString = builder.Configuration.GetConnectionString("InfinipayDBContext");
            connection = new SqlConnection(connectionString);
        }

        protected DataTable ExecuteQuery(string statement)
        {
            using var command = new SqlCommand(statement, connection);
            var adapter = new SqlDataAdapter(command);
            var table = new DataTable();
            connection.Open();
            adapter.Fill(table);
            return table;
        }

        protected void ExecuteCommand(string statement)
        {
            using var command = new SqlCommand(statement, connection);
            connection.Open();
            bool success = command.ExecuteNonQuery() >= 1;
            connection.Close();
            if (!success)
            {
                throw new Exception("SQL: 'ExecuteCommand' failed.");
            }
        }
    }
}
