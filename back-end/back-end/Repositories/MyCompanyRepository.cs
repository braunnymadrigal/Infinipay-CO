using System.Data;
using back_end.Models;
using Microsoft.Data.SqlClient;

namespace back_end.Repositories
{
    public class MyCompanyRepository
    {
        private readonly SqlConnection _connection;
        private readonly string? _pathConnection;

        public MyCompanyRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _pathConnection = builder.Configuration.GetConnectionString("InfinipayDBContext");
            _connection = new SqlConnection(_pathConnection);
        }

        public MyCompanyModel Get(string ownerId)
        {
            MyCompanyModel myCompanyModel = new MyCompanyModel();
            return myCompanyModel;
        }

        private DataTable CreateTable(string query)
        {
            SqlCommand command = new SqlCommand(query, _connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            _connection.Open();
            adapter.Fill(table);
            _connection.Close();
            return table;
        }
    }
}
