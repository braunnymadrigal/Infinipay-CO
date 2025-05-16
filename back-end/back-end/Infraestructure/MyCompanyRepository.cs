using back_end.Models;

using System.Data;
using Microsoft.Data.SqlClient;
using System;

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
                query = $"SELECT * FROM [Direccion] WHERE [idPersona] = '{companyId}';";
                myCompanyModel = FillFullAddress(myCompanyModel, query);
                query = $"SELECT * FROM [PersonaJuridica] WHERE [id] = '{companyId}';";
                myCompanyModel = FillLegalPerson(myCompanyModel, query);
                query = $"SELECT * FROM [Persona] WHERE [id] = '{companyId}';";
                myCompanyModel = FillPerson(myCompanyModel, query);
            }
            return myCompanyModel;
        }

        private MyCompanyModel FillPerson(MyCompanyModel myCompanyModel, string query)
        {
            DataTable table = CreateTable(query);
            if (table.Rows.Count > 0)
            {
                DataRow rowResult = table.Rows[0];
                var email = Convert.ToString(rowResult["correoElectronico"]);
                var phone = Convert.ToString(rowResult["numeroTelefono"]);
                var document = Convert.ToString(rowResult["identificacion"]);
                var birth = Convert.ToDateTime(rowResult["fechaNacimiento"]);
                if (email != null && phone != null && document != null && birth != DateTime.MinValue)
                {
                    myCompanyModel.Email = email;
                    myCompanyModel.Phone = phone;
                    myCompanyModel.Document = document;
                    myCompanyModel.Birth = Convert.ToString(birth.ToString("dd-MM-yyyy"));
                }
            }
            return myCompanyModel;
        }

        private MyCompanyModel FillLegalPerson(MyCompanyModel myCompanyModel, string query)
        {
            DataTable table = CreateTable(query);
            if (table.Rows.Count > 0)
            {
                DataRow rowResult = table.Rows[0];
                var name = Convert.ToString(rowResult["razonSocial"]);
                var description = Convert.ToString(rowResult["descripcion"]);
                var paymentType = Convert.ToString(rowResult["tipoPago"]);
                var benefits = Convert.ToString(rowResult["beneficiosPorEmpleado"]);
                if (name != null && description != null && paymentType != null && benefits != null)
                {
                    myCompanyModel.Name = name;
                    myCompanyModel.Description = description;
                    myCompanyModel.PaymentType = paymentType;
                    myCompanyModel.Benefits = benefits;
                }
            }
            return myCompanyModel;
        }

        private MyCompanyModel FillFullAddress(MyCompanyModel myCompanyModel, string query)
        {
            DataTable table = CreateTable(query);
            if (table.Rows.Count > 0)
            {
                DataRow rowResult = table.Rows[0];
                var province = Convert.ToString(rowResult["provincia"]);
                var canton = Convert.ToString(rowResult["canton"]);
                var district = Convert.ToString(rowResult["distrito"]);
                var address = Convert.ToString(rowResult["otrasSenas"]);
                if (province != null && canton != null && district != null && address != null)
                {
                    myCompanyModel.Province = province;
                    myCompanyModel.Canton = canton;
                    myCompanyModel.District = district;
                    if (address != "")
                    {
                        myCompanyModel.Address = address;
                    }
                }
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
                        fullName = firstGivenName + " " + secondGivenName + " " + firstFamilyName + " " + secondFamilyName;
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
