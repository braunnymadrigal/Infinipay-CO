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

    private bool GetAssignmentResult(string query, SqlParameter[] parameters)
    {
      SqlCommand queryCommand = new SqlCommand(query, _connection);
      queryCommand.Parameters.AddRange(parameters);

      _connection.Open();
      bool assignmentResultSuccess = queryCommand.ExecuteNonQuery() >= 1;
      _connection.Close();

      return assignmentResultSuccess;
    }
    public List<AssignedBenefitListModel> GetBenefits(string userNickname)
    {
      List<AssignedBenefitListModel> benefitsList = new 
        List<AssignedBenefitListModel>();

      string query = @"
        SELECT
            b.id,
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
            pj.beneficiosPorEmpleado,
            CASE 
                WHEN bpe.idEmpleado IS NOT NULL THEN 1
                ELSE 0
            END AS isAssigned
        FROM Beneficio b
        JOIN PersonaJuridica pj ON pj.id = b.idPersonaJuridica
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
          benefitId = column["id"] != DBNull.Value ? (Guid)column["id"] : Guid.Empty,
          benefitName = column["nombre"] != DBNull.Value ? Convert.ToString(column["nombre"]) : null,
          benefitMinTime = column["tiempoMinimo"] != DBNull.Value ? Convert.ToDecimal(column["tiempoMinimo"]) : 0,
          benefitDescription = column["descripcion"] != DBNull.Value ? Convert.ToString(column["descripcion"]) : null,
          benefitElegibleEmployees = column["empleadoElegible"] != DBNull.Value ? Convert.ToString(column["empleadoElegible"]) : null,
          formulaType = column["tipoFormula"] != DBNull.Value ? Convert.ToString(column["tipoFormula"]) : null,
          urlAPI = column["urlAPI"] != DBNull.Value ? Convert.ToString(column["urlAPI"]) : null,
          formulaParamUno = column["paramUno"] != DBNull.Value ? Convert.ToString(column["paramUno"]) : null,
          formulaParamDos = column["paramDos"] != DBNull.Value ? Convert.ToString(column["paramDos"]) : null,
          formulaParamTres = column["paramTres"] != DBNull.Value ? Convert.ToString(column["paramTres"]) : null,
          userCreator = column["usuarioCreador"] != DBNull.Value ? Convert.ToString(column["usuarioCreador"]) : null,
          creationDate = column["fechaCreacion"] != DBNull.Value ? Convert.ToDateTime(column["fechaCreacion"]) : DateTime.MinValue,
          userModifier = column["ultimoUsuarioModificador"] != DBNull.Value ? Convert.ToString(column["ultimoUsuarioModificador"]) : null,
          modifiedDate = column["ultimaFechaModificacion"] != DBNull.Value ? Convert.ToDateTime(column["ultimaFechaModificacion"]) : (DateTime?)null,
          asignado = column["isAssigned"] != DBNull.Value && Convert.ToBoolean(column["isAssigned"]),
          beneficiosPorEmpleado = column["beneficiosPorEmpleado"] != DBNull.Value ? Convert.ToInt16(column["beneficiosPorEmpleado"]) : (short)0
        });
      }
      return benefitsList;
    }

    public bool AssignBenefit(AssignBenefitRequest request)
    {
      string query = @"
        INSERT INTO BeneficioPorEmpleado ([idBeneficio], [idEmpleado])
        VALUES(@BeneficioId, @PersonaFisicaId)";

      SqlParameter[] parameters = new SqlParameter[]
        {
        new SqlParameter("@PersonaFisicaId", request.userPersonId),
        new SqlParameter("@BeneficioId", request.benefitId)

        };

      return GetAssignmentResult(query, parameters);
    }
  }
}
