using System.Data;
using back_end.Domain;
using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
  public class AdministratorReportRepository : IAdministratorReportRepository
  {
    private readonly AbstractConnectionRepository connectionRepository;

    public AdministratorReportRepository(AbstractConnectionRepository
      connectionRepository)
    {
      this.connectionRepository = connectionRepository;
    }

    private string getAllCompaniesPayrollQuery()
    {
        return @"
          BEGIN TRY
              BEGIN TRAN;
              WITH PagosLey AS (
                  SELECT
                      emp.idPersonaFisica AS empleadorId,
                      pj.razonSocial AS empresaNombre,
                      pj.tipoPago AS frecuenciaPago,
                      dp.fechaInicio,
                      dp.fechaFin,
                      SUM(ROUND(dp.salarioBruto / 10.0, 0)) AS totalSalarios,
                      SUM(COALESCE(dap.monto, 0)) AS totalPagosLeyEmpleador
                  FROM Empleador emp
                  JOIN PersonaJuridica pj ON pj.id = emp.idPersonaJuridica
                  JOIN Empleado e ON e.idEmpleadorContratador = emp.idPersonaFisica
                  JOIN PersonaFisica pfE ON pfE.id = emp.idPersonaFisica
                  JOIN Contrato c ON c.idEmpleado = e.idPersonaFisica
                  JOIN DetallePago dp ON dp.idEmpleado = e.idPersonaFisica
                  LEFT JOIN DeduccionAPago dap
                         ON dap.idDetallePago = dp.id
                        AND dap.tipo LIKE 'empleador_%'
                  WHERE dp.fechaInicio >= @startDate
                    AND dp.fechaFin    <= @endDate
                  GROUP BY
                      emp.idPersonaFisica,
                      pfE.primerNombre,  pfE.segundoNombre,
                      pfE.primerApellido, pfE.segundoApellido,
                      pj.razonSocial,    pj.tipoPago,
                      dp.fechaInicio,    dp.fechaFin
              ),

              DeduccionesVol AS (
                  SELECT
                      emp.idPersonaFisica AS empleadorId,
                      dp.fechaInicio,
                      SUM(dap.monto) AS totalDeduccionesVoluntarias
                  FROM DeduccionAPago dap
                  JOIN DetallePago dp ON dp.id = dap.idDetallePago
                  JOIN Empleado e ON e.idPersonaFisica = dp.idEmpleado
                  JOIN Empleador emp ON emp.idPersonaFisica = e.idEmpleadorContratador
                  WHERE dap.tipo NOT LIKE 'empleador_%'
                    AND dp.fechaInicio >= @startDate
                    AND dp.fechaFin    <= @endDate
                  GROUP BY emp.idPersonaFisica, dp.fechaInicio
              )

              SELECT
                  p.empresaNombre,
                  p.frecuenciaPago,
                  p.fechaInicio,
                  p.fechaFin,
                  p.totalSalarios,
                  p.totalPagosLeyEmpleador,
                  ISNULL(d.totalDeduccionesVoluntarias, 0) AS totalDeduccionesVoluntarias
              FROM PagosLey AS p
              LEFT JOIN DeduccionesVol AS d
                     ON d.empleadorId = p.empleadorId
                    AND d.fechaInicio = p.fechaInicio
              ORDER BY
                  p.empresaNombre,
                  p.fechaInicio DESC;

              COMMIT TRAN;
          END TRY
          BEGIN CATCH
              IF @@TRANCOUNT > 0
                  ROLLBACK TRAN;
              THROW;
          END CATCH;
         ";
    }
    private List<PayrollAdministratorModel> getAllCompaniesPayrollFromTable(
      DataTable table)
    {
      var companyPayroll = new List<PayrollAdministratorModel>();

      try
      {
        if (table.Rows.Count == 0) return companyPayroll;

        foreach (DataRow row in table.Rows)
        {
          var grossEmployerTax = row["totalPagosLeyEmpleador"] != DBNull.Value
            ? Convert.ToDouble(row["totalPagosLeyEmpleador"]) : double.MinValue;

          var grossSalary = row["totalSalarios"] != DBNull.Value
              ? Convert.ToDouble(row["totalSalarios"]) : double.MinValue;

          companyPayroll.Add(new PayrollAdministratorModel
          {
            companyName = row["empresaNombre"] != DBNull.Value
              ? Convert.ToString(row["empresaNombre"]) : string.Empty,

            hiringType = row["frecuenciaPago"] != DBNull.Value
              ? Convert.ToString(row["frecuenciaPago"]) : string.Empty,

            startDate = row["fechaInicio"] != DBNull.Value
              ? DateOnly.FromDateTime(Convert.ToDateTime(row["fechaInicio"]))
              : DateOnly.FromDateTime(DateTime.MinValue),

            endDate = row["fechaFin"] != DBNull.Value
              ? DateOnly.FromDateTime(Convert.ToDateTime(row["fechaFin"]))
              : DateOnly.FromDateTime(DateTime.MinValue),

            payDate = row["fechaFin"] != DBNull.Value
              ? DateOnly.FromDateTime(Convert.ToDateTime(row["fechaFin"]))
              : DateOnly.FromDateTime(DateTime.MinValue),

            grossSalary = grossSalary,

            grossEmployerTax = grossEmployerTax,

            voluntaryDeductionsTotal = row["totalDeduccionesVoluntarias"]
            != DBNull.Value ? Convert.ToDouble(row
            ["totalDeduccionesVoluntarias"]) : double.MinValue,

            totalEmployerCost = grossEmployerTax + grossSalary,
          });
        }
        return companyPayroll;
      }
      catch (Exception ex)
      {
        throw new Exception("Error obteniendo datos.");
      }
    }

    public List<PayrollAdministratorModel> getAllCompaniesPayroll(DateOnly
      startDate, DateOnly endDate)
    {
      var query = getAllCompaniesPayrollQuery();

      try
      {
        var command = new SqlCommand(query, connectionRepository.connection);

        command.Parameters.AddWithValue("@startDate", startDate);
        command.Parameters.AddWithValue("@endDate", endDate);
        var resultTable = connectionRepository.ExecuteQuery(command);
        return getAllCompaniesPayrollFromTable(resultTable);
      }
      catch (Exception)
      {
        throw new Exception("Error obteniendo datos de planillas.");
      }
    }
  }
}
