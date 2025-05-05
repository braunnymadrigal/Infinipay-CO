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
        builder.Configuration.GetConnectionString("InfinipayDBContext");
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

    private DataTable GetQueryTable(string query, SqlParameter[] parameters)
    {
      SqlCommand queryCommand = new SqlCommand(query, _connection);
      queryCommand.Parameters.AddRange(parameters);
      SqlDataAdapter tableAdapter = new SqlDataAdapter(queryCommand);
      DataTable queryTable = new DataTable();
      _connection.Open();
      tableAdapter.Fill(queryTable);
      _connection.Close();
      return queryTable;
    }

    public List<AssignedBenefitListModel> GetBenefits(string userNickname)
    {
      List<AssignedBenefitListModel> benefitsList = new 
        List<AssignedBenefitListModel>();

      string query = @"
        SELECT 
            b.nombre,
            b.tiempoMinimo,
            b.descripcion,
            b.empleadoElegible,
            f.tipoFormula,
            f.urlAPI,
            f.paramUno,
            f.paramDos,
            f.paramTres,
            a.usuarioCreador,
            a.fechaCreacion,
            a.ultimoUsuarioModificador,
            a.ultimaFechaModificacion,
            CASE 
                WHEN bpe.idEmpleado IS NOT NULL THEN 1
                ELSE 0
            END AS isAssigned
        FROM Beneficio b
        JOIN Empleador em ON em.idPersonaJuridica = b.idPersonaJuridica
        JOIN Auditoria a ON a.id = b.idAuditoria
        JOIN Empleado e ON e.idEmpleadorContratador = em.idPersonaFisica
        JOIN Usuario u ON u.idPersonaFisica = e.idPersonaFisica
        JOIN Deduccion d ON d.idBeneficio = b.id
        JOIN Formula f ON f.id = d.idFormula
        LEFT JOIN BeneficioPorEmpleado bpe 
               ON bpe.idEmpleado = e.idPersonaFisica AND bpe.idBeneficio = b.id
        WHERE u.nickname = @nickname
          AND (e.rol = b.empleadoElegible OR b.empleadoElegible = 'todos');
    ";

      SqlParameter[] parameters = new SqlParameter[]
      {
        new SqlParameter("@nickname", userNickname)
      };

      DataTable tableResult = GetQueryTable(query, parameters);

      foreach (DataRow column in tableResult.Rows)
      {
        benefitsList.Add(new AssignedBenefitListModel
        {
          benefitName = Convert.ToString(column["nombre"]),
          benefitMinTime = Convert.ToDecimal(column["tiempoMinimo"]),
          benefitDescription = Convert.ToString(column["descripcion"]),
          benefitElegibleEmployees =
            Convert.ToString(column["empleadoElegible"]),
          formulaType = Convert.ToString(column["tipoFormula"]),
          urlAPI = column["urlAPI"] != DBNull.Value ? Convert.ToString(column["urlAPI"]) : null,
          formulaParamUno = Convert.ToString(column["paramUno"]),
          formulaParamDos = column["paramDos"] != DBNull.Value ? Convert.ToString(column["paramDos"]) : null,
          formulaParamTres = column["paramTres"] != DBNull.Value ? Convert.ToString(column["paramTres"]) : null,
          userCreator = Convert.ToString(column["usuarioCreador"]),
          creationDate = Convert.ToDateTime(column["fechaCreacion"]),
          userModifier = column["ultimoUsuarioModificador"] != DBNull.Value ? Convert.ToString(column["ultimoUsuarioModificador"]) : null,
          modifiedDate = column["ultimaFechaModificacion"] != DBNull.Value ? Convert.ToDateTime(column["ultimaFechaModificacion"]) : (DateTime?)null,
          isAssigned = Convert.ToBoolean(column["isAssigned"])
        });
      }

      return benefitsList;
    }
  }
}
