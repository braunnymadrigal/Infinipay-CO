using back_end.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Microsoft.Data.SqlClient;
namespace back_end.Repositories
{
    public class BenefitRepository
    {
        private readonly string _connectionRoute;

        private void mapBenefit(BenefitModel benefit, DataRow row)
        {
            benefit.Id = (int)row["IdBeneficio"];
            benefit.Name = (string)row["Nombre"];
            benefit.MinMonths = (decimal)row["MesesMinimos"];
            benefit.Description = (string)row["Descripcion"];
            benefit.ElegibleEmployee = (string)row["EmpleadoElegible"];
            benefit.legalName = (string)row["NombreLegal"];
            benefit.deductionType = (string)row["TipoDeduccion"];
            benefit.payment = (int)row["Pago"];
        }
        public BenefitRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionRoute = builder.Configuration.GetConnectionString("InfinipayDBContext");
        }
        private SqlConnection GetConnection() {
            return new SqlConnection(_connectionRoute);
        }
        private DataTable getQueryTable(string query)
        {
            using (var connection = GetConnection())
            using (var queryCommand = new SqlCommand(query, connection))
            using (var tableAdapter = new SqlDataAdapter(queryCommand))
            {
                var queryTable = new DataTable();
                connection.Open();
                tableAdapter.Fill(queryTable);
                return queryTable;
            }
        }
        private bool dataAlreadyExists(string table, string field, string value
            , SqlTransaction transaction)
        {
            var cmd = new SqlCommand(
                $"SELECT COUNT(*) FROM [{table}] WHERE [{field}] = @value",
                transaction.Connection, transaction);
                cmd.Parameters.AddWithValue("@value", value);
            var count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
        public List<BenefitModel> GetAllBenefits()
        {
            var query = "SELECT * FROM Beneficio";
            var table = getQueryTable(query);
            var benefits = new List<BenefitModel>();
            foreach (DataRow row in table.Rows)
            {
                var benefit = new BenefitModel();
                mapBenefit(benefit, row);
                benefits.Add(benefit);
            }
            return benefits;
        }
        public BenefitModel GetBenefitById(int id)
        {
            var query = $"SELECT * FROM Beneficio WHERE IdBeneficio = {id}";
            var table = getQueryTable(query);
            if (table.Rows.Count == 0) return null;
            var row = table.Rows[0];
            var benefit = new BenefitModel();
            mapBenefit(benefit, row);
            return benefit;
        }
        public bool CreateBenefit(BenefitModel benefit)
        {
            var query = @"INSERT INTO Beneficio 
                (Nombre, MesesMinimos, Descripcion, EmpleadoElegible, NombreLegal, TipoDeduccion, Pago)
                VALUES (@Nombre, @MesesMinimos, @Descripcion, @EmpleadoElegible, @NombreLegal, @TipoDeduccion, @Pago)";
    
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", benefit.Name);
                    command.Parameters.AddWithValue("@MesesMinimos", benefit.MinMonths);
                    command.Parameters.AddWithValue("@Descripcion", benefit.Description);
                    command.Parameters.AddWithValue("@EmpleadoElegible", benefit.ElegibleEmployee);
                    command.Parameters.AddWithValue("@NombreLegal", benefit.legalName);
                    command.Parameters.AddWithValue("@TipoDeduccion", benefit.deductionType);
                    command.Parameters.AddWithValue("@Pago", benefit.payment);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }


    }
}
