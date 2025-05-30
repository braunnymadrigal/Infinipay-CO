using Microsoft.Data.SqlClient;
using System.Data;

namespace back_end.Infraestructure
{
    public abstract class AbstractConnectionRepository
    {
        protected readonly SqlConnection connection;
        protected readonly string connectionString;

        protected AbstractConnectionRepository()
        {
            connectionString = BuildConnectionString("InfinipayDBContext");
            connection = new SqlConnection(connectionString);
        }

        public DataTable ExecuteQuery(SqlCommand command)
        {
            var adapter = new SqlDataAdapter(command);
            var table = new DataTable();
            connection.Open();
            adapter.Fill(table);
            connection.Close();
            return table;
        }

        public void ExecuteCommand(SqlCommand command)
        {
            connection.Open();
            var success = command.ExecuteNonQuery() >= 1;
            connection.Close();
            if (!success)
            {
                throw new Exception("SQL: 'ExecuteCommand' failed.");
            }
        }

        private string BuildConnectionString(string connectionStringContext)
        {
            var builder = WebApplication.CreateBuilder();
            var currentConnectionString = builder.Configuration.GetConnectionString(connectionStringContext);
            if (currentConnectionString == null)
            {
                throw new Exception("SQL: 'BuildConnectionString' failed.");
            }
            return currentConnectionString;
        }
    }
}
