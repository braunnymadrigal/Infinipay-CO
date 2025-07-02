using back_end.Domain;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace back_end.Infraestructure
{
  public class EmployerReportRepository : IEmployerReportRepository
  {
    private readonly AbstractConnectionRepository connectionRepository;

    public EmployerReportRepository(
        AbstractConnectionRepository connectionRepository)
    {
      this.connectionRepository = connectionRepository;
    }

    public EmployerPayrollReport GetEmployerPayrollReport(string employerId)
    {
      using var connection = connectionRepository.connection;
      connection.Open();
      using var transaction = connection.BeginTransaction();
      try
      {
        var rows = executeEmployerQuery(connection, transaction, employerId);
        var voluntaryDeductions = executeVoluntaryDeductionsSumQuery(connection
          , transaction, employerId);
        transaction.Commit();

        foreach (var period in rows)
        {
          var periodoKey = (DateTime)period["fechaInicio"];
          if (voluntaryDeductions.TryGetValue(periodoKey
            , out decimal deducciones))
            period["total_deducciones_voluntarias"] = deducciones;
          else
            period["total_deducciones_voluntarias"] = 0m;
        }

        return buildEmployerReport(rows);
      }
      catch (Exception ex)
      {
        transaction.Rollback();
        throw new Exception("GetEmployerPayrollReport failed: " + ex.Message
          , ex);
      }
    }

    private List<Dictionary<string, object>> executeEmployerQuery(
      SqlConnection conn, SqlTransaction tx, string employerId)
    {
      const string sql = @"
        SELECT
          pfE.primerNombre AS empPrimerNombre,
          pfE.segundoNombre AS empSegundoNombre,
          pfE.primerApellido AS empPrimerApellido,
          pfE.segundoApellido AS empSegundoApellido,
          pj.razonSocial AS empresaNombre,
          dp.fechaInicio,
          dp.fechaFin,
          SUM(CASE WHEN c.tipoContrato = 'horas' THEN ROUND(dp.salarioBruto/ 10.0, 0) ELSE 0 END) AS totalHoras,
          SUM(CASE WHEN c.tipoContrato = 'tiempoCompleto' THEN ROUND(dp.salarioBruto/ 10.0, 0) ELSE 0 END) AS totalTiempoCompleto,
          SUM(CASE WHEN c.tipoContrato = 'servicios' THEN ROUND(dp.salarioBruto/ 10.0, 0) ELSE 0 END) AS totalServicios,
          SUM(CASE WHEN c.tipoContrato = 'medioTiempo' THEN ROUND(dp.salarioBruto/ 10.0, 0) ELSE 0 END) AS totalMedioTiempo,
          SUM(ROUND(dp.salarioBruto/ 10.0, 0)) AS totalSalarios,
          SUM(CASE WHEN dap.tipo = 'empleador_ccss_ivm' THEN dap.monto ELSE 0 END) AS total_empleador_ccss_ivm,
          SUM(CASE WHEN dap.tipo = 'empleador_ccss_sem' THEN dap.monto ELSE 0 END) AS total_empleador_ccss_sem,
          SUM(CASE WHEN dap.tipo = 'empleador_lpt_bpop' THEN dap.monto ELSE 0 END) AS total_empleador_lpt_bpop,
          SUM(CASE WHEN dap.tipo = 'empleador_lpt_opc' THEN dap.monto ELSE 0 END) AS total_empleador_lpt_opc,
          SUM(CASE WHEN dap.tipo = 'empleador_lpt_fcl' THEN dap.monto ELSE 0 END) AS total_empleador_lpt_fcl,
          SUM(CASE WHEN dap.tipo = 'empleador_lpt_ins' THEN dap.monto ELSE 0 END) AS total_empleador_lpt_ins,
          SUM(CASE WHEN dap.tipo = 'empleador_otras_ina' THEN dap.monto ELSE 0 END) AS total_empleador_otras_ina,
          SUM(CASE WHEN dap.tipo = 'empleador_otras_imas' THEN dap.monto ELSE 0 END) AS total_empleador_otras_imas,
          SUM(CASE WHEN dap.tipo = 'empleador_otras_familiares' THEN dap.monto ELSE 0 END) AS total_empleador_otras_familiares,
          SUM(CASE WHEN dap.tipo = 'empleador_otras_bpop' THEN dap.monto ELSE 0 END) AS total_empleador_otras_bpop
        FROM Empleador emp
        JOIN PersonaJuridica pj ON pj.id = emp.idPersonaJuridica
        JOIN Empleado e ON e.idEmpleadorContratador = emp.idPersonaFisica
        JOIN PersonaFisica pfE ON pfE.id = emp.idPersonaFisica
        JOIN Contrato c ON c.idEmpleado = e.idPersonaFisica
        JOIN DetallePago dp ON dp.idEmpleado = e.idPersonaFisica
        LEFT JOIN DeduccionAPago dap ON dap.idDetallePago = dp.id AND dap.tipo LIKE 'empleador_%'
        WHERE emp.idPersonaFisica = @employerId
        GROUP BY
          pfE.primerNombre,
          pfE.segundoNombre,
          pfE.primerApellido,
          pfE.segundoApellido,
          pj.razonSocial,
          dp.fechaInicio,
          dp.fechaFin
        ORDER BY dp.fechaInicio DESC;
    ";

      using var cmd = new SqlCommand(sql, conn, tx);
      cmd.Parameters.AddWithValue("@employerId", Guid.Parse(employerId));

      var list = new List<Dictionary<string, object>>();
      using var rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        list.Add(Enumerable.Range(0, rdr.FieldCount)
            .ToDictionary(rdr.GetName, i => rdr.IsDBNull(i)
            ? null : rdr.GetValue(i)));
      }
      return list;
    }

    private Dictionary<DateTime, decimal>
      executeVoluntaryDeductionsSumQuery(SqlConnection conn, SqlTransaction tx
      , string employerId)
    {
      const string sql = @"
        SELECT
          dp.fechaInicio,
          SUM(dap.monto) AS totalVoluntaryDeductions
        FROM DeduccionAPago dap
        JOIN DetallePago dp ON dp.id = dap.idDetallePago
        JOIN Empleado e ON e.idPersonaFisica = dp.idEmpleado
        WHERE e.idEmpleadorContratador = @employerId
          AND dap.tipo NOT LIKE 'empleador_%'
        GROUP BY dp.fechaInicio;
    ";

      using var cmd = new SqlCommand(sql, conn, tx);
      cmd.Parameters.AddWithValue("@employerId", Guid.Parse(employerId));

      var dict = new Dictionary<DateTime, decimal>();
      using var rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        var fechaInicio = (DateTime)rdr["fechaInicio"];
        var totalDeductions =
          rdr.IsDBNull(rdr.GetOrdinal("totalVoluntaryDeductions"))
          ? 0m : (decimal)rdr["totalVoluntaryDeductions"];
        dict[fechaInicio] = totalDeductions;
      }
      return dict;
    }

    private EmployerPayrollReport buildEmployerReport(List<Dictionary<string,
      object>> rows)
    {
      if (rows.Count == 0) return null;

      var first = rows[0];
      var report = new EmployerPayrollReport
      {
        employerFullName = $"{first["empPrimerNombre"]}" +
        $"  {(first["empSegundoNombre"] ?? "")} {first["empPrimerApellido"]}" +
        $"  {first["empSegundoApellido"]}".Trim(),
        companyName = (string)first["empresaNombre"],
        periodSummaries = new List<PayPeriodSummary>()
      };

      foreach (var r in rows)
      {
        report.periodSummaries.Add(new PayPeriodSummary
        {
          startDate = (DateTime)r["fechaInicio"],
          endDate = (DateTime)r["fechaFin"],
          totalEmpHoursSalary = (decimal)r["totalHoras"],
          totalEmpFullTimeSalary = (decimal)r["totalTiempoCompleto"],
          totalEmpHalfTimeSalary = (decimal)r["totalMedioTiempo"],
          totalEmpServicesSalary = (decimal)r["totalServicios"],
          totalSalaries = (decimal)r["totalSalarios"],

          totalEmployerCcssIvm = (decimal)r["total_empleador_ccss_ivm"],
          totalEmployerCcssSem = (decimal)r["total_empleador_ccss_sem"],
          totalEmployerLptBpop = (decimal)r["total_empleador_lpt_bpop"],
          totalEmployerLptOpc = (decimal)r["total_empleador_lpt_opc"],
          totalEmployerLptFcl = (decimal)r["total_empleador_lpt_fcl"],
          totalEmployerLptIns = (decimal)r["total_empleador_lpt_ins"],
          totalEmployerOtrasIna = (decimal)r["total_empleador_otras_ina"],
          totalEmployerOtrasImas = (decimal)r["total_empleador_otras_imas"],
          totalEmployerOtrasFamiliares 
            = (decimal)r["total_empleador_otras_familiares"],
          totalEmployerOtrasBpop = (decimal)r["total_empleador_otras_bpop"],

          totalEmployeeVoluntaryDeductions =
          r.ContainsKey("total_deducciones_voluntarias")
          ? (decimal)r["total_deducciones_voluntarias"] : 0m
        });
      }

      return report;
    }

  }
}