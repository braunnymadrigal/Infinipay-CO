using System.Data;
using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
    public abstract class GeneralRepository
    {
        protected readonly SqlConnection connection;
        private readonly string? connectionString;

        protected GeneralRepository()
        {
            var builder = WebApplication.CreateBuilder();
            connectionString = builder.Configuration.GetConnectionString("InfinipayDBContext");
            connection = new SqlConnection(connectionString);
        }

        protected DataTable ExecuteQuery(SqlCommand command)
        {
            var adapter = new SqlDataAdapter(command);
            var table = new DataTable();
            connection.Open();
            adapter.Fill(table);
            connection.Close();
            return table;
        }

        protected void ExecuteCommand(SqlCommand command)
        {
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
