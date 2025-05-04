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
            string query = $"SELECT [idPersonaJuridica] FROM [Empleador] WHERE [idPersonaFisica] = '{ownerId}';";
            string companyId = "";
            companyId = GetFirstColumnFirstRow(query);
            if (companyId != "")
            {
                query = $"SELECT * FROM [PersonaFisica] WHERE [id] = '{ownerId}';";
                myCompanyModel = FillPhysicalPerson(myCompanyModel, query);
            }
            return myCompanyModel;
        }

        private MyCompanyModel FillPhysicalPerson(MyCompanyModel myCompanyModel, string query)
        {
            string fullName = "";
            DataTable table = CreateTable(query);
            if (table.Rows.Count > 0)
            {
                DataRow rowResult = table.Rows[0];
                var firstGivenName = Convert.ToString(rowResult["primerNombre"]);
                var secondGivenName = Convert.ToString(rowResult["segundoNombre"]);
                var firstFamilyName = Convert.ToString(rowResult["primerApellido"]);
                var secondFamilyName = Convert.ToString(rowResult["segundoApellido"]);
                if (firstGivenName != null && secondGivenName != null 
                    && firstFamilyName != null && secondFamilyName != null)
                {
                    if (secondGivenName != "")
                    {
                        fullName = firstGivenName + " " + secondGivenName + firstFamilyName + " " + secondFamilyName;
                    }
                    else
                    {
                        fullName = firstGivenName + " " + firstFamilyName + " " + secondFamilyName;
                    }
                    myCompanyModel.Owner = fullName;
                }
            }
            return myCompanyModel;
        }

        private string GetFirstColumnFirstRow(string query)
        {
            string returnValue = "";
            DataTable table = CreateTable(query);
            if (table.Rows.Count > 0)
            {
                DataRow rowResult = table.Rows[0];
                var cellResult = Convert.ToString(rowResult[0]);
                if (cellResult != "" && cellResult != null)
                {
                    returnValue = cellResult;
                }
            }
            return returnValue;
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
