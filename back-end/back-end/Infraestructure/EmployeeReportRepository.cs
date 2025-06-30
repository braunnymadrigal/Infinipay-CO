using back_end.Domain;
using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
  public class EmployeeReportRepository : IEmployeeReportRepository
  {
    private readonly AbstractConnectionRepository connectionRepository;
    private readonly IUtilityRepository utilityRepository;

    public EmployeeReportRepository(
        AbstractConnectionRepository connectionRepository,
        IUtilityRepository utilityRepository)
    {
      this.connectionRepository = connectionRepository;
      this.utilityRepository = utilityRepository;
    }

    public EmployeeFullReport GetEmployeePayrollReport(string employeeId)
    {
      using var connection = connectionRepository.connection;
      connection.Open();

      using var transaction = connection.BeginTransaction();

      try
      {
        var rows = executeEmployeeQuery(connection, transaction, employeeId);
        transaction.Commit();

        var result = buildEmployeeReport(rows);
        return result;
      }
      catch (Exception ex)
      {
        transaction.Rollback();
        throw new Exception("GetEmployeePayrollReport failed: " + ex.Message
          , ex);
      }
    }

    private List<Dictionary<string, object>> executeEmployeeQuery(
      SqlConnection connection, SqlTransaction transaction, string employeeId)
    {
      const string query = @"
      SELECT 
          pf.primerNombre,
          pf.segundoNombre,
          pf.primerApellido,
          pf.segundoApellido,
          c.tipoContrato,
          pj.razonSocial AS empresa,
          dp.salarioBruto,
          dp.salarioNeto,
          dp.fechaInicio,
          dp.fechaFin,
          dap.monto,
          dap.tipo,
          d.nombre AS nombreDeduccion
      FROM Empleado e
      INNER JOIN PersonaFisica pf ON pf.id = e.idPersonaFisica
      INNER JOIN Contrato c ON c.idEmpleado = e.idPersonaFisica
      INNER JOIN Empleador emp ON emp.idPersonaFisica = e.idEmpleadorContratador
      INNER JOIN PersonaJuridica pj ON pj.id = emp.idPersonaJuridica
      INNER JOIN DetallePago dp ON dp.idEmpleado = e.idPersonaFisica
      LEFT JOIN DeduccionAPago dap ON dap.idDetallePago = dp.id
      LEFT JOIN Deduccion d ON d.id = dap.idDeduccion
      WHERE e.idPersonaFisica = @employeeId
      ORDER BY dp.fechaInicio DESC;";

      var dataBaseRows = new List<Dictionary<string, object>>();

      using var command = new SqlCommand(query, connection, transaction);
      command.Parameters.AddWithValue("@employeeId", Guid.Parse(employeeId));

      using var reader = command.ExecuteReader();

      while (reader.Read())
      {
        var row = Enumerable.Range(0, reader.FieldCount)
            .ToDictionary(reader.GetName, i => reader.IsDBNull(i) ? null
            : reader.GetValue(i));
        dataBaseRows.Add(row);
      }

      return dataBaseRows;
    }

    private EmployeeFullReport buildEmployeeReport(List<Dictionary<string
      , object>> rows)
    {
      if (rows.Count == 0)
        return null;

      var firstRow = rows[0];

      var report = new EmployeeFullReport
      {
        fullName = $"{firstRow["primerNombre"]} {(firstRow["segundoNombre"] ??
        "")} {firstRow["primerApellido"]} {firstRow["segundoApellido"]}".Trim(),
        contractType = (string)firstRow["tipoContrato"],
        companyName = (string)firstRow["empresa"],
        payments = new List<PaymentReport>()
      };

      var paymentMap = new Dictionary<string, PaymentReport>();

      foreach (var row in rows)
      {
        var paymentKey =
          $"{((DateTime)row["fechaInicio"]).Ticks}-{((DateTime)row["fechaFin"]).Ticks}";

        if (!paymentMap.ContainsKey(paymentKey))
        {
          var payment = new PaymentReport
          {
            computedGrossSalary = (decimal)row["salarioBruto"],
            netSalary = (decimal)row["salarioNeto"],
            startDate = (DateTime)row["fechaInicio"],
            endDate = (DateTime)row["fechaFin"],
            deductions = new List<DeductionDetail>()
          };
          paymentMap[paymentKey] = payment;
          report.payments.Add(payment);
        }

        if (row["monto"] != null && row["tipo"] != null)
        {
          string tipo = ((string)row["tipo"]).ToLower();

          if (!tipo.StartsWith("empleador_"))
          {
            string deductionName = row["nombreDeduccion"]?.ToString();

            if (string.IsNullOrWhiteSpace(deductionName))
            {
              deductionName = tipo switch
              {
                "empleado_renta" => "Impuesto de Renta",
                "empleado_ccss_ivm" => "Invalidez, Vejez y Muerte (IVM)",
                "empleado_ccss_sem" => "Seguro de Enfermedad y Maternidad (SEM)",
                "empleado_lpt_bpop" => "Aporte Trabajador Banco Popular",
                _ => "Deducción",
              };
            }

            paymentMap[paymentKey].deductions.Add(new DeductionDetail
            {
              amount = (decimal)row["monto"],
              type = (string)row["tipo"],
              deductionName = deductionName
            });
          }
        }
      }

      report.payments = report.payments
        .OrderByDescending(p => p.startDate)
        .Take(10)
        .ToList();

      return report;
    }
  }
}