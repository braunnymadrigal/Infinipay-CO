using back_end.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace back_end.Repositories
{
  public class AssignedBenefitListRepository
  {
    private SqlConnection _connection;
    private string _connectionRoute;

    public AssignedBenefitListRepository()
    {
      var builder = WebApplication.CreateBuilder();
      _connectionRoute =
        builder.Configuration.GetConnectionString("InfinipayCO");
      _connection = new SqlConnection(_connectionRoute);
    }

    private DataTable GetQueryTable(string query)
    {
      SqlCommand queryCommand = new SqlCommand(query, _connection);
      SqlDataAdapter tableAdapter = new SqlDataAdapter(queryCommand);
      DataTable queryTable = new DataTable();
      _connection.Open();
      tableAdapter.Fill(queryTable);
      _connection.Close();
      return queryTable;
    }

    public List<AssignedBenefitListModel> GetBenefits()
    {
      List<AssignedBenefitListModel> benefitsList
        = new List<AssignedBenefitListModel>();
      string query = "SELECT * FROM [dbo].[Beneficio]";
      DataTable tableResult = GetQueryTable(query);

      foreach (DataRow column in tableResult.Rows)
      {
        benefitsList.Add(
          new AssignedBenefitListModel
          {
            benefitId = (Guid)column["id"],
            benefitName = Convert.ToString(column["nombre"]),
            benefitMinTime = Convert.ToDecimal(column["tiempoMinimo"]),
            benefitDescription = Convert.ToString(column["descripcion"]),
            benefitElegibleEmployees =
              Convert.ToString(column["empleadoElegible"]),

            benefitAudit = (Guid)column["idAuditoria"],
            companyId = (Guid)column["idPersonaJuridica"]
          });
      }
      return benefitsList;
    }

  }
}