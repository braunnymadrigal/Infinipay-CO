using back_end.Domain;
using back_end.Infraestructure;
using Microsoft.Data.SqlClient;

public class PayrollOrchestratorRepository
{
  private readonly AbstractConnectionRepository connectionRepository;
  private readonly IUtilityRepository utilityRepository;

  public PayrollOrchestratorRepository(
      AbstractConnectionRepository connectionRepository,
      IUtilityRepository utilityRepository)
  {
    this.connectionRepository = connectionRepository;
    this.utilityRepository = utilityRepository;
  }

  public void SavePayrollData(
      string employerId,
      DateOnly startDate,
      DateOnly endDate,
      List<EmployeePayrollResult> payrollResults)
  {
    using var connection = connectionRepository.connection;
    connection.Open();
    using var transaction = connection.BeginTransaction();

    try
    {
      var employerUsername = obtainEmployerUsername(employerId, connection
        , transaction);

      var employerLegalId = obtainEmployerLegalId(employerId, connection
        , transaction);
      var auditTime = DateTime.UtcNow;

      var auditId = insertAuditData(employerUsername, auditTime
        , connection, transaction);
      var payrollId = insertPayroll(auditId, employerLegalId, startDate
        , endDate, connection, transaction);

      foreach (var employee in payrollResults)
      {
        processEmployeePayroll(employee, startDate, endDate, payrollId
          , connection, transaction);
      }

      transaction.Commit();
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw new Exception("SavePayrollData failed: " + ex.Message, ex);
    }
  }
  private string obtainEmployerUsername(string employerId
    , SqlConnection connection, SqlTransaction transaction)
  {
    var command = new SqlCommand(@"
        SELECT u.nickname FROM Usuario u
        JOIN PersonaFisica pf ON pf.id = u.idPersonaFisica
        JOIN Empleador e ON e.idPersonaFisica = pf.id
        WHERE e.idPersonaFisica = @employerId", connection, transaction);
    command.Parameters.AddWithValue("@employerId", Guid.Parse(employerId));
    return (string)command.ExecuteScalar();
  }

  private Guid obtainEmployerLegalId(string employerId
    , SqlConnection connection, SqlTransaction transaction)
  {
    var command = new SqlCommand(@"
        SELECT idPersonaJuridica FROM Empleador WHERE idPersonaFisica = @employerId"
      , connection, transaction);
    command.Parameters.AddWithValue("@employerId", Guid.Parse(employerId));
    return (Guid)command.ExecuteScalar();
  }

  private void processEmployeePayroll(
      EmployeePayrollResult employee,
      DateOnly startDate,
      DateOnly endDate,
      Guid payrollId,
      SqlConnection connection,
      SqlTransaction transaction)
  {
    insertEmployeeOfPayroll(payrollId, employee.EmployeeId, connection, transaction);

    var paymentDetails = insertPaymentDetails(payrollId, employee, startDate
      , endDate, connection, transaction);

    var taxes = employee.EmployeeTaxes;
    var employerTaxes = employee.EmployerTaxes;

    void InsertEmployeeTax(string tipo, double monto)
    {
      if (monto > 0)
        insertDeduction(null, paymentDetails, monto, tipo, connection
          , transaction);
    }

    InsertEmployeeTax("empleado_renta", taxes.employeeRent);
    InsertEmployeeTax("empleado_ccss_ivm", taxes.employeeCcssIvm);
    InsertEmployeeTax("empleado_ccss_sem", taxes.employeeCcssSem);
    InsertEmployeeTax("empleado_lpt_bpop", taxes.employeeLptBpop);

    foreach (var ded in employee.Deductions)
    {
      if (Guid.TryParse(ded.id, out var parsedId))
      {
        insertDeduction(parsedId, paymentDetails, ded.resultAmount
          , "empleado_beneficio", connection, transaction);
      }
    }

    void InsertEmployerTax(string tipo, double monto)
    {
      if (monto > 0)
        insertDeduction(null, paymentDetails, monto, tipo, connection
          , transaction);
    }

    InsertEmployerTax("empleador_ccss_sem", employerTaxes.employerCcssSem);
    InsertEmployerTax("empleador_ccss_ivm", employerTaxes.employerCcssIvm);
    InsertEmployerTax("empleador_otras_bpop", employerTaxes.employerOthersBpop);
    InsertEmployerTax("empleador_otras_familiares", employerTaxes.employerOthersFamily);
    InsertEmployerTax("empleador_otras_imas", employerTaxes.employerOthersImas);
    InsertEmployerTax("empleador_otras_ina", employerTaxes.employerOthersIna);
    InsertEmployerTax("empleador_lpt_bpop", employerTaxes.employerLptBpop);
    InsertEmployerTax("empleador_lpt_fcl", employerTaxes.employerLptFcl);
    InsertEmployerTax("empleador_lpt_opc", employerTaxes.employerLptOpc);
    InsertEmployerTax("empleador_lpt_ins", employerTaxes.employerLptIns);
  }

  private Guid insertAuditData(string employerUsername, DateTime auditDate
    , SqlConnection connection, SqlTransaction transaction)
  {
    var id = Guid.NewGuid();
    var command = new SqlCommand(@"
        INSERT INTO Auditoria (id, fechaCreacion, usuarioCreador)
        VALUES (@id, @fechaCreacion, @usuarioCreador)", connection
        , transaction);
    command.Parameters.AddWithValue("@id", id);
    command.Parameters.AddWithValue("@fechaCreacion", auditDate);
    command.Parameters.AddWithValue("@usuarioCreador", employerUsername);
    command.ExecuteNonQuery();
    return id;
  }

  private Guid insertPayroll(Guid auditId, Guid employerLegalPersonId
    , DateOnly startDate, DateOnly endDate, SqlConnection connection, SqlTransaction transaction)
  {
    var id = Guid.NewGuid();
    var command = new SqlCommand(@"
        INSERT INTO Planilla (id, idAuditoria, idPersonaJuridica, fechaInicio, fechaFin, estado)
        VALUES (@id, @idAuditoria, @idPersonaJuridica, @fechaInicio, @fechaFin, @estado)"
, connection, transaction);
    command.Parameters.AddWithValue("@id", id);
    command.Parameters.AddWithValue("@idAuditoria", auditId);
    command.Parameters.AddWithValue("@idPersonaJuridica", employerLegalPersonId);
    command.Parameters.AddWithValue("@fechaInicio", startDate.ToDateTime(TimeOnly.MinValue));
    command.Parameters.AddWithValue("@fechaFin", endDate.ToDateTime(TimeOnly.MinValue));
    command.Parameters.AddWithValue("@estado", "completado");
    command.ExecuteNonQuery();
    return id;
  }

  private void insertEmployeeOfPayroll(Guid payrollId, Guid employeeId
    , SqlConnection connection, SqlTransaction transaction)
  {
    var command = new SqlCommand(@"
        INSERT INTO EmpleadoDePlanilla (idPlanilla, idEmpleado)
        VALUES (@idPlanilla, @idEmpleado)", connection, transaction);
    command.Parameters.AddWithValue("@idPlanilla", payrollId);
    command.Parameters.AddWithValue("@idEmpleado", employeeId);
    command.ExecuteNonQuery();
  }

  private Guid insertPaymentDetails(Guid payrollId,
    EmployeePayrollResult employee,
    DateOnly startDate,
    DateOnly endDate,
    SqlConnection connection,
    SqlTransaction transaction)
  {
    var id = Guid.NewGuid();
    var command = new SqlCommand(@"
      INSERT INTO DetallePago (id, idPlanilla, idEmpleado, fechaInicio, fechaFin, salarioBruto, salarioNeto)
      VALUES (@id, @idPlanilla, @idEmpleado, @fechaInicio, @fechaFin, @salarioBruto, @salarioNeto)",
      connection, transaction);

    command.Parameters.AddWithValue("@id", id);
    command.Parameters.AddWithValue("@idPlanilla", payrollId);
    command.Parameters.AddWithValue("@idEmpleado", employee.EmployeeId);
    command.Parameters.AddWithValue("@fechaInicio", startDate.ToDateTime(TimeOnly.MinValue));
    command.Parameters.AddWithValue("@fechaFin", endDate.ToDateTime(TimeOnly.MinValue));
    command.Parameters.AddWithValue("@salarioBruto", employee.ComputedGrossSalary);
    command.Parameters.AddWithValue("@salarioNeto", employee.NetSalary);

    command.ExecuteNonQuery();
    return id;
  }


  private void insertDeduction(Guid? deductionId, Guid paymentDetailId
    , double paymentAmount, string deductionType, SqlConnection connection
    , SqlTransaction transaction)
  {
    Guid deductionPaymentId = Guid.NewGuid();
    var insert = new SqlCommand(@"
        INSERT INTO DeduccionAPago (idDeduccion, idDetallePago,id, monto, tipo)
        VALUES (@idDeduccion, @idDetallePago,@id, @monto, @tipo)"
        , connection, transaction);

    insert.Parameters.AddWithValue("@id", deductionPaymentId);
    insert.Parameters.AddWithValue("@idDetallePago", paymentDetailId);
    insert.Parameters.AddWithValue("@monto", paymentAmount);
    insert.Parameters.AddWithValue("@tipo", deductionType);

    if (deductionId.HasValue)
      insert.Parameters.AddWithValue("@idDeduccion", deductionId.Value);
    else
      insert.Parameters.AddWithValue("@idDeduccion", DBNull.Value);

    insert.ExecuteNonQuery();
  }
  public List<object> getPayrollsByEmployerId(string employerId)
  {
    using var connection = connectionRepository.connection;
    connection.Open();

    using var transaction = connection.BeginTransaction();

    try
    {
      var dataBaseRows = executePayrollQuery(connection, transaction
        , employerId);

      transaction.Commit();

      return buildPayrollResults(dataBaseRows);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw new Exception("GetPayrollsByEmployerId failed: " + ex.Message, ex);
    }
  }

  private List<Dictionary<string, object>>
    executePayrollQuery(SqlConnection connection, SqlTransaction transaction
    , string employerId)
  {
    var query = @"
      SELECT 
          p.id AS PlanillaId,
          p.fechaInicio,
          p.fechaFin,
          p.estado,
          pf.id AS PersonaFisicaId,
          pf.primerNombre,
          pf.segundoNombre,
          pf.primerApellido,
          pf.segundoApellido,
          dp.salarioBruto,
          dp.salarioNeto,
          dp.id AS DetallePagoId,
          dap.monto,
          dap.tipo,
          d.nombre AS nombreDeduccion -- <-- nombre del beneficio (si aplica)
      FROM Planilla p
      INNER JOIN EmpleadoDePlanilla ep ON ep.idPlanilla = p.id
      INNER JOIN DetallePago dp ON dp.idPlanilla = p.id AND dp.idEmpleado = ep.idEmpleado
      INNER JOIN Empleado e ON e.idPersonaFisica = ep.idEmpleado
      INNER JOIN PersonaFisica pf ON pf.id = e.idPersonaFisica
      LEFT JOIN DeduccionAPago dap ON dap.idDetallePago = dp.id
      LEFT JOIN Deduccion d ON dap.idDeduccion = d.id -- <--- JOIN aquí
      WHERE p.idPersonaJuridica = (
          SELECT idPersonaJuridica FROM Empleador WHERE idPersonaFisica = @employerId
      )
      ORDER BY p.fechaInicio DESC, pf.primerApellido ASC, pf.segundoApellido ASC";

    var dataBaseRows = new List<Dictionary<string, object>>();

    using var command = new SqlCommand(query, connection, transaction);
    command.Parameters.AddWithValue("@employerId", Guid.Parse(employerId));

    using var reader = command.ExecuteReader();

    while (reader.Read())
    {
      var dataBaseRow = Enumerable.Range(0, reader.FieldCount)
          .ToDictionary(reader.GetName, i => reader.IsDBNull(i) ? null :
          reader.GetValue(i));
      dataBaseRows.Add(dataBaseRow);
    }

    return dataBaseRows;
  }

  private List<object> buildPayrollResults(List<Dictionary<string, object>>
    dataBaseRows)
  {
    var payrollMap = new Dictionary<Guid, PayrollResult>();
    var filteredEmployees = new Dictionary<Guid, EmployeeResult>();

    foreach (var row in dataBaseRows)
    {
      var payrollId = (Guid)row["PlanillaId"];
      var paymentDetailsId = (Guid)row["DetallePagoId"];

      if (!payrollMap.ContainsKey(payrollId))
      {
        payrollMap[payrollId] = createPayrollResult(row);
      }

      if (!filteredEmployees.ContainsKey(paymentDetailsId))
      {

        filteredEmployees[paymentDetailsId] = createEmployeeResult(row);
        payrollMap[payrollId].payrollEmployees.Add(filteredEmployees[paymentDetailsId]);
      }

      string tipo = (string)row["tipo"];
      if (!tipo.StartsWith("empleador_"))
      {
        string displayName;
        if (tipo == "empleado_beneficio" && row.ContainsKey("nombreDeduccion") && row["nombreDeduccion"] != null)
        {
          displayName = row["nombreDeduccion"].ToString();
        }
        else
        {
          displayName = DeductionDisplayNames.ContainsKey(tipo)
            ? DeductionDisplayNames[tipo]
            : tipo;
        }

        filteredEmployees[paymentDetailsId].addDeduction(new DeductionResult
        {
          deductionAmount = (decimal)row["monto"],
          deductionType = displayName
        });
      }
    }

    return payrollMap.Values.Cast<object>().ToList();
  }

  private PayrollResult createPayrollResult(Dictionary<string, object> row)
  {
    return new PayrollResult
    {
      payrollId = (Guid)row["PlanillaId"],
      payrollStartDate = (DateTime)row["fechaInicio"],
      payrollEndDate = (DateTime)row["fechaFin"],
      payrollStatus = (string)row["estado"],
      payrollEmployees = new List<EmployeeResult>()
    };
  }

  private EmployeeResult createEmployeeResult(Dictionary<string, object> row)
  {
    var completeName = $"{row["primerNombre"]} {(row["segundoNombre"] ?? "")}"
      + $"{row["primerApellido"]} {row["segundoApellido"]}".Trim();

    return new EmployeeResult
    {
      employeeName = completeName,
      employeeComputedGrossSalary = (decimal)row["salarioBruto"],
      employeeNetSalary = (decimal)row["salarioNeto"]
    };
  }

  private static readonly Dictionary<string, string>
    DeductionDisplayNames = new()
    {

      ["empleado_ccss_ivm"] = "Invalidez, Vejez y Muerte (IVM)",
      ["empleado_ccss_sem"] = "Seguro de Enfermedad y Maternidad (SEM)",
      ["empleado_renta"] = "Impuesto de Renta",
      ["empleado_lpt_bpop"] = "Aporte Trabajador Banco Popular",

      ["empleador_ccss_ivm"] = "Invalidez, Vejez y Muerte (IVM)",
      ["empleador_ccss_sem"] = "Seguro de Enfermedad y Maternidad (SEM)",
      ["empleador_lpt_bpop"] = "Cuota Patronal Banco Popular (0.25%)",
      ["empleador_lpt_opc"] = "Fondo de Pensiones Complementarias (0.50%)",
      ["empleador_lpt_fcl"] = "Fondo de Capitalización Laboral (FCL) (3.00%)",
      ["empleador_lpt_ins"] = "INS (1.00%)",
      ["empleador_otras_ina"] = "INA (1.50%)",
      ["empleador_otras_imas"] = "IMAS (0.50%) ",
      ["empleador_otras_familiares"] = "Asignaciones Familiares  (5.00%)",
      ["empleador_otras_bpop"] = "Aporte Banco Popular (0.25%)"
    };

}