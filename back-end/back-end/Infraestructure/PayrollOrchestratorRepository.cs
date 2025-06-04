using System.Diagnostics;
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
    insertEmployeeOfPayroll(payrollId, employee.EmployeeId, connection
      , transaction);

    var paymentDetails = insertPaymentDetails(payrollId, employee, startDate
      , endDate, connection, transaction);

    if (employee.RentTax > 0)
      insertDeduction(null, paymentDetails, employee.RentTax, "renta"
        , connection, transaction);

    if (employee.CcssTax > 0)
      insertDeduction(null, paymentDetails, employee.CcssTax, "ccssEmpleado"
        , connection, transaction);

    foreach (var ded in employee.Deductions)
    {
      if (Guid.TryParse(ded.id, out var parsedId))
      {
        insertDeduction(parsedId, paymentDetails, ded.resultAmount
          , "beneficio", connection, transaction);
      }
    }
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
    , DateOnly startDate, DateOnly endDate, SqlConnection connection
    , SqlTransaction transaction)
  {
    var id = Guid.NewGuid();
    var command = new SqlCommand(@"
        INSERT INTO Planilla (id, idAuditoria, idPersonaJuridica, fechaInicio, fechaFin, estado)
        VALUES (@id, @idAuditoria, @idPersonaJuridica, @fechaInicio, @fechaFin, @estado)"
          , connection, transaction);
    command.Parameters.AddWithValue("@id", id);
    command.Parameters.AddWithValue("@idAuditoria", auditId);
    command.Parameters.AddWithValue("@idPersonaJuridica"
      , employerLegalPersonId);
    command.Parameters.AddWithValue("@fechaInicio"
      , startDate.ToDateTime(TimeOnly.MinValue));
    command.Parameters.AddWithValue("@fechaFin"
      , endDate.ToDateTime(TimeOnly.MinValue));
    command.Parameters.AddWithValue("@estado", "en progreso");
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

  private Guid insertPaymentDetails(Guid payrollId
    , EmployeePayrollResult employee, DateOnly startDate, DateOnly endDate
    , SqlConnection connection, SqlTransaction transaction)
  {
    var id = Guid.NewGuid();
    var command = new SqlCommand(@"
        INSERT INTO DetallePago (id, idPlanilla, idEmpleado, fechaInicio, fechaFin, salarioBruto, salarioNeto)
        VALUES (@id, @idPlanilla, @idEmpleado, @fechaInicio, @fechaFin, @salarioBruto, @salarioNeto)"
        , connection, transaction);
    command.Parameters.AddWithValue("@id", id);
    command.Parameters.AddWithValue("@idPlanilla", payrollId);
    command.Parameters.AddWithValue("@idEmpleado", employee.EmployeeId);
    command.Parameters.AddWithValue("@fechaInicio"
      , startDate.ToDateTime(TimeOnly.MinValue));
    command.Parameters.AddWithValue("@fechaFin"
      , endDate.ToDateTime(TimeOnly.MinValue));
    command.Parameters.AddWithValue("@salarioBruto"
      , employee.ComputedGrossSalary);
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
            dap.tipo
        FROM Planilla p
        INNER JOIN EmpleadoDePlanilla ep ON ep.idPlanilla = p.id
        INNER JOIN DetallePago dp ON dp.idPlanilla = p.id AND dp.idEmpleado = ep.idEmpleado
        INNER JOIN Empleado e ON e.idPersonaFisica = ep.idEmpleado
        INNER JOIN PersonaFisica pf ON pf.id = e.idPersonaFisica
        LEFT JOIN DeduccionAPago dap ON dap.idDetallePago = dp.id
        WHERE p.idPersonaJuridica = (
            SELECT idPersonaJuridica FROM Empleador WHERE idPersonaFisica = @employerId
        )
        ORDER BY p.fechaInicio DESC, pf.primerApellido ASC";

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

      if (row["monto"] != null && row["tipo"] != null)
      {
        filteredEmployees[paymentDetailsId].addDeduction(new DeductionResult
        {
          deductionAmount = (decimal)row["monto"],
          deductionType = (string)row["tipo"]
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
      employeeGrossSalary = (decimal)row["salarioBruto"],
      employeeNetSalary = (decimal)row["salarioNeto"]
    };
  }
}
