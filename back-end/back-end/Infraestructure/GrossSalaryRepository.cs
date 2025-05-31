using System.Data;
using back_end.Domain;
using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
  public class GrossSalaryRepository : IGrossSalaryRepository
  {
    private readonly AbstractConnectionRepository connectionRepository;
    private readonly IUtilityRepository utilityRepository;

    public GrossSalaryRepository(AbstractConnectionRepository connectionRepository
        , IUtilityRepository utilityRepository)
    {
      this.connectionRepository = connectionRepository;
      this.utilityRepository = utilityRepository;
    }

    public List<GrossSalaryModel> GetGrossSalaries(string employerId, DateOnly startDate, DateOnly endDate)
    {
      var command = CreateGrossSalaryTableCommand(employerId, startDate, endDate);
      var dataTable = connectionRepository.ExecuteQuery(command);
      var grossSalaries = TransformDataTableInGrossSalaryList(dataTable);
      return grossSalaries;
    }

    private List<GrossSalaryModel> TransformDataTableInGrossSalaryList(DataTable dataTable)
    {
      if (dataTable.Rows.Count <= 0)
      {
        throw new Exception("Gross salary table could not be extracted.");
      }
      List<GrossSalaryModel> grossSalaryModels = new List<GrossSalaryModel>();
      foreach (DataRow dataRow in dataTable.Rows)
      {
        var currentGrossSalaryModel = TransformDataRowInGrossSalaryModel(dataRow);
        grossSalaryModels.Add(currentGrossSalaryModel);
      }
      return grossSalaryModels;
    }

    private GrossSalaryModel TransformDataRowInGrossSalaryModel(DataRow dataRow)
    {
      var employeeId = utilityRepository.ConvertDatabaseValueToString(dataRow["id"]);
      var firstName = utilityRepository.ConvertDatabaseValueToString(dataRow["primerNombre"]);
      var lastName1 = utilityRepository.ConvertDatabaseValueToString(dataRow["primerApellido"]);
      var lastName2 = utilityRepository.ConvertDatabaseValueToString(dataRow["segundoApellido"]);
      var fullName = $"{firstName} {lastName1} {lastName2}".Trim();

      var hiringDate = utilityRepository.ConvertDatabaseValueToString(dataRow["fechaContratacion"]);
      var grossSalary = utilityRepository.ConvertDatabaseValueToString(dataRow["salarioBruto"]);
      var hiringType = utilityRepository.ConvertDatabaseValueToString(dataRow["tipoContrato"]);
      var hoursDate = utilityRepository.ConvertDatabaseValueToString(dataRow["fechaHoras"]);
      var hoursWorked = utilityRepository.ConvertDatabaseValueToString(dataRow["horasTrabajadas"]);
      var actualHoursDate = hoursDate != "" ? DateOnly.FromDateTime(Convert.ToDateTime(hoursDate)) : DateOnly.MinValue;
      var actualHoursWorked = hoursWorked != "" ? Convert.ToInt32(hoursWorked) : 0;
      var actualGrossSalary = Convert.ToDouble(grossSalary);
      return new GrossSalaryModel
      {
        EmployeeId = employeeId,
        EmployeeName = fullName,
        HiringDate = DateOnly.FromDateTime(Convert.ToDateTime(hiringDate)),
        ComputedGrossSalary = actualGrossSalary,
        GrossSalary = actualGrossSalary,
        HiringType = hiringType,
        HoursDate = actualHoursDate,
        HoursWorked = actualHoursWorked,
      };
    }

    private SqlCommand CreateGrossSalaryTableCommand(string employerId, DateOnly startDate, DateOnly endDate)
    {
      var query = CreateGrossSalaryTableQuery();
      var command = new SqlCommand(query, connectionRepository.connection);
      command.Parameters.AddWithValue("@employerId", employerId);
      command.Parameters.AddWithValue("@startDate", startDate);
      command.Parameters.AddWithValue("@endDate", endDate);
      return command;
    }

    private string CreateGrossSalaryTableQuery()
    {
      var query = @"
        SELECT 
            e.[idPersonaFisica] as id,
            p.[primerNombre] as primerNombre,
            p.[primerApellido] as primerApellido,
            p.[segundoApellido] as segundoApellido,
            e.[fechaContratacion] as fechaContratacion,
            c.[salarioBruto] as salarioBruto,
            c.[tipoContrato] as tipoContrato,
            h.[fecha] as fechaHoras,
            h.[horasTrabajadas] as horasTrabajadas
        FROM [Empleado] as e
        INNER JOIN [PersonaFisica] as p ON e.[idPersonaFisica] = p.[id]
        FULL OUTER JOIN [Contrato] as c ON c.[idEmpleado] = e.[idPersonaFisica]
        FULL OUTER JOIN [Horas] as h ON h.[idEmpleado] = e.[idPersonaFisica] AND
            h.[fecha] = (
                SELECT fechaHoras
                FROM function_getEmployeeCurrentHours(e.[idPersonaFisica], @startDate, @endDate)
            )
        WHERE e.[idEmpleadorContratador] = @employerId AND e.[fechaDespido] IS NULL;";
      return query;
    }
  }
}