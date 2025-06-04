using back_end.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using back_end.Application;

namespace back_end.Repositories
{
  public class EmployeeBenefitRepository :
    IBenefitRepository<EmployeeBenefitDTO>
  {
    private SqlConnection _connection;
    private string _connectionRoute;
    private string _connectStringName = "InfinipayDBContext";
    public EmployeeBenefitRepository() 
    {
      var builder = WebApplication.CreateBuilder();

      _connectionRoute = builder.Configuration
        .GetConnectionString(_connectStringName)
          ?? throw new Exception("Connection string not found.");

      _connection = new SqlConnection(_connectionRoute);
    }

    private DataTable GetQueryTable(string query, SqlParameter[] parameters)
    {
      DataTable queryTable = new DataTable();

      try
      {
        using (SqlCommand queryCommand = new SqlCommand(query, _connection))
        using (SqlDataAdapter tableAdapter = new SqlDataAdapter(queryCommand))
        {
          if (parameters != null && parameters.Length > 0)
          {
            queryCommand.Parameters.AddRange(parameters);
          }

          if (_connection.State == ConnectionState.Open)
          {
            _connection.Open();
          }

          tableAdapter.Fill(queryTable);
        }
      }
      catch (Exception ex)
      {
        _connection.Close();
        throw new Exception("Error al ejecutar consulta con parámetros: "
          + ex.Message, ex);
      }

      _connection.Close();
      return queryTable;
    }

    private string getBenefitsQuery()
    {
      return @"
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
            END AS asignado
        FROM Beneficio b
        JOIN PersonaJuridica pj ON pj.id = b.idPersonaJuridica
        JOIN Empleador em ON em.idPersonaJuridica = b.idPersonaJuridica
        JOIN Auditoria a ON a.id = b.idAuditoria
        JOIN Empleado e ON e.idEmpleadorContratador = em.idPersonaFisica
        JOIN Usuario u ON u.idPersonaFisica = e.idPersonaFisica
        JOIN Deduccion d ON d.idBeneficio = b.id
        JOIN Formula f ON f.id = d.idFormula
		    JOIN Contrato c ON c.idEmpleado = e.idPersonaFisica
        LEFT JOIN BeneficioPorEmpleado bpe 
               ON bpe.idEmpleado = e.idPersonaFisica AND bpe.idBeneficio = b.id
        WHERE u.nickname = 'juanito77'
          AND (c.tipoContrato = b.empleadoElegible OR b.empleadoElegible
            = 'todos');
      ";
    }

    private void getBenefitsFromTable(List<EmployeeBenefitDTO> benefits
      , DataTable tableResult)
    {
      foreach (DataRow column in tableResult.Rows)
      {
        benefits.Add(new EmployeeBenefitDTO
        {
          benefit = new BenefitDTO
          {
            id = column["id"] != DBNull.Value
              ? (Guid)column["id"] : Guid.Empty,

            name = column["nombre"] != DBNull.Value
              ? Convert.ToString(column["nombre"]) : string.Empty,

            minEmployeeTime = column["tiempoMinimo"] != DBNull.Value
              ? Convert.ToDecimal(column["tiempoMinimo"]) : 0,

            description = column["descripcion"] != DBNull.Value
              ? Convert.ToString(column["descripcion"]) : string.Empty,

            elegibleEmployees = column["empleadoElegible"] != DBNull.Value
              ? Convert.ToString(column["empleadoElegible"]) : string.Empty,

            deductionType = column["tipoFormula"] != DBNull.Value
              ? Convert.ToString(column["tipoFormula"]) : string.Empty,

            urlAPI = column["urlAPI"] != DBNull.Value
              ? Convert.ToString(column["urlAPI"]) : string.Empty,

            paramOneAPI = column["paramUno"] != DBNull.Value
            ? Convert.ToString(column["paramUno"]) : string.Empty,

            paramTwoAPI = column["paramDos"] != DBNull.Value
              ? Convert.ToString(column["paramDos"]) : string.Empty,

            paramThreeAPI = column["paramTres"] != DBNull.Value
              ? Convert.ToString(column["paramTres"]) : string.Empty,

            userCreator = column["usuarioCreador"] != DBNull.Value
              ? Convert.ToString(column["usuarioCreador"]) : string.Empty,

            creationDate = column["fechaCreacion"] != DBNull.Value
              ? Convert.ToDateTime(column["fechaCreacion"])
                : DateTime.MinValue,

            userModifier = column["ultimoUsuarioModificador"] != DBNull.Value
              ? Convert.ToString(column["ultimoUsuarioModificador"])
                : string.Empty,

            modifiedDate = column["ultimaFechaModificacion"] != DBNull.Value
              ? Convert.ToDateTime(column["ultimaFechaModificacion"])
                : DateTime.MinValue,

            benefitsPerEmployee = column["beneficiosPorEmpleado"]
              != DBNull.Value
                ? Convert.ToInt16(column["beneficiosPorEmpleado"]) : (short)0
          },
          assigned = column["asignado"] != DBNull.Value
            && Convert.ToBoolean(column["asignado"])
        });
      }
    }

    public List<EmployeeBenefitDTO> getBenefits(string loggedUserNickname)
    {
      List<EmployeeBenefitDTO> benefits = new List<EmployeeBenefitDTO>();

      string query = getBenefitsQuery();

      try
      { 
        SqlParameter[] parameters = new SqlParameter[] {
          new SqlParameter("@nickname", loggedUserNickname)
        };

        DataTable tableResult = GetQueryTable(query, parameters);

        getBenefitsFromTable(benefits, tableResult);
      }
      catch (Exception ex)
      {
        throw new Exception("Error al obtener lista de beneficios"
          + ex.Message, ex);
      }

      return benefits;
    }

    private bool getAssignmentResult(string query, SqlParameter[] parameters)
    {
      bool assignmentResultSuccess = false;

      try
      {
        using (SqlCommand queryCommand = new SqlCommand(query, _connection))
        {
          if (parameters != null && parameters.Length > 0)
          {
            queryCommand.Parameters.AddRange(parameters);
          }

          if (_connection.State != ConnectionState.Open)
          {
            _connection.Open();
          }
          assignmentResultSuccess = queryCommand.ExecuteNonQuery() >= 1;
        }
      }
      catch (Exception ex)
      {
        _connection.Close();
        throw new Exception("Error al ejecutar consulta de asignación de " +
          "beneficio: " + ex.Message, ex);
      }
      _connection.Close();

      return assignmentResultSuccess;
    }

    private string assignBenefitQuery()
    {
      return @"
        INSERT INTO BeneficioPorEmpleado ([idBeneficio], [idEmpleado])
        VALUES(@BeneficioId, @PersonaFisicaId)";
    }

    public bool assignBenefit(AssignBenefitReq request
      , string loggedUserNickname)
    {
      string query = assignBenefitQuery();

      SqlParameter[] parameters =
        [
        new SqlParameter("@PersonaFisicaId", loggedUserNickname),
        new SqlParameter("@BeneficioId", request.id)
        ];

      try
      {
        return getAssignmentResult(query, parameters);
      }
      catch (Exception ex)
      {
        throw new Exception("Error al intentar asignar beneficio a empleado" +
          ex.Message, ex);
      }
    }
  }
}
