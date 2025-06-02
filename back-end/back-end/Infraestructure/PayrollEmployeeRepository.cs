using System.Data;
using back_end.Application;
using back_end.Domain;
using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
    public class PayrollEmployeeRepository : IPayrollEmployeeRepository
    {
        private const int MONTHS_TO_SUBSTRACT = -1;
        private const int PAYROLL_EMPLOYEE_LIST_INITIAL_INDEX = -1;

        private readonly AbstractConnectionRepository connectionRepository;
        private readonly IUtilityRepository utilityRepository;

        public PayrollEmployeeRepository(AbstractConnectionRepository connectionRepository
            , IUtilityRepository utilityRepository)
        {
            this.connectionRepository = connectionRepository;
            this.utilityRepository = utilityRepository;
        }

        public List<PayrollEmployeeModel> getPayrollEmployees(string employerId, DateOnly startDate, DateOnly endDate)
        {
            var command = createPayrollEmployeeTableCommand(employerId, startDate, endDate);
            var dataTable = connectionRepository.ExecuteQuery(command);
            var payrollEmployees = transformDataTablePayrollEmployeeList(dataTable);
            return payrollEmployees;
        }

        private List<PayrollEmployeeModel> transformDataTablePayrollEmployeeList(DataTable dataTable)
        {
            checkDataTableCorrectness(dataTable);
            var payrollEmployees = new List<PayrollEmployeeModel>();
            var payrollEmployeesIndex = PAYROLL_EMPLOYEE_LIST_INITIAL_INDEX;
            var previousId = "";
            var deductionIds = new HashSet<string>();
            var payrollIds = new HashSet<string>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var id = utilityRepository.ConvertDatabaseValueToString(dataRow["id"]);
                var deductionId = utilityRepository.ConvertDatabaseValueToString(dataRow["deductionId"]);
                var payrollId = utilityRepository.ConvertDatabaseValueToString(dataRow["payrollId"]);
                if (previousId != id)
                {
                    ++payrollEmployeesIndex;
                    payrollEmployees = addPayrollEmployeeModel(payrollEmployees, payrollEmployeesIndex, dataRow);
                    previousId = id;
                    deductionIds.Clear();
                    payrollIds.Clear();
                }
                if (deductionIds.Contains(deductionId) == false)
                {
                    payrollEmployees = addPayrollDeductionModel(payrollEmployees, payrollEmployeesIndex, dataRow);
                    deductionIds.Add(deductionId);
                }
                if (payrollIds.Contains(payrollId) == false)
                {
                    payrollEmployees = addPreviousComputedSalary(payrollEmployees, payrollEmployeesIndex, dataRow);
                    payrollIds.Add(payrollId);
                }
            }
            return payrollEmployees;
        }

        private List<PayrollEmployeeModel> addPayrollEmployeeModel(List<PayrollEmployeeModel> payrollEmployees
            , int payrollEmployeesIndex, DataRow dataRow)
        {
            var id = utilityRepository.ConvertDatabaseValueToString(dataRow["id"]);
            var birthDate = utilityRepository.ConvertDatabaseValueToString(dataRow["birthDate"]);
            var gender = utilityRepository.ConvertDatabaseValueToString(dataRow["gender"]);
            var salary = utilityRepository.ConvertDatabaseValueToString(dataRow["salary"]);
            var hiringType = utilityRepository.ConvertDatabaseValueToString(dataRow["hiringType"]);
            var hiringDate = utilityRepository.ConvertDatabaseValueToString(dataRow["hiringDate"]);
            var hoursDate = utilityRepository.ConvertDatabaseValueToString(dataRow["hoursDate"]);
            var hoursNumber = utilityRepository.ConvertDatabaseValueToString(dataRow["hoursNumber"]);
            var companyAssociaton = utilityRepository.ConvertDatabaseValueToString(dataRow["companyAssociation"]);
            var actualHoursDate = hoursDate != "" ? DateOnly.FromDateTime(Convert.ToDateTime(hoursDate)) : DateOnly.MinValue;
            var actualHoursNumber = hoursNumber != "" ? Convert.ToInt32(hoursNumber) : 0;
            var newPayrollEmployee = new PayrollEmployeeModel
            {
                id = id,
                birthDate = DateOnly.FromDateTime(Convert.ToDateTime(birthDate)),
                gender = gender,
                rawGrossSalary = Convert.ToDouble(salary),
                computedGrossSalary = 0,
                ccssEmployeeDeduction = 0,
                ccssEmployerDeduction = 0,
                hiringType = hiringType,
                hiringDate = DateOnly.FromDateTime(Convert.ToDateTime(hiringDate)),
                hoursDate = actualHoursDate,
                hoursNumber = actualHoursNumber,
                companyAssociation = companyAssociaton,
                deductions = new List<PayrollDeductionModel>(),
                previousComputedGrossSalaries = new List<PayrollPreviousComputedGrossSalary>()
            };
            payrollEmployees.Add(newPayrollEmployee);
            return payrollEmployees;
        }

        private List<PayrollEmployeeModel> addPayrollDeductionModel(List<PayrollEmployeeModel> payrollEmployees
            , int payrollEmployeesIndex, DataRow dataRow)
        {
            var deductionId = utilityRepository.ConvertDatabaseValueToString(dataRow["deductionId"]);
            if (deductionId != "")
            {
                var dependantNumber = utilityRepository.ConvertDatabaseValueToString(dataRow["dependantNumber"]);
                var formulaType = utilityRepository.ConvertDatabaseValueToString(dataRow["formulaType"]);
                var apiUrl = utilityRepository.ConvertDatabaseValueToString(dataRow["apiUrl"]);
                var apiMethod = utilityRepository.ConvertDatabaseValueToString(dataRow["apiMethod"]);
                var param1Value = utilityRepository.ConvertDatabaseValueToString(dataRow["param1Value"]);
                var param2Value = utilityRepository.ConvertDatabaseValueToString(dataRow["param2Value"]);
                var param3Value = utilityRepository.ConvertDatabaseValueToString(dataRow["param3Value"]);
                var param1Key = utilityRepository.ConvertDatabaseValueToString(dataRow["param1Key"]);
                var param2Key = utilityRepository.ConvertDatabaseValueToString(dataRow["param2Key"]);
                var param3Key = utilityRepository.ConvertDatabaseValueToString(dataRow["param3Key"]);
                var header1Value = utilityRepository.ConvertDatabaseValueToString(dataRow["header1Value"]);
                var header1Key = utilityRepository.ConvertDatabaseValueToString(dataRow["header1Key"]);
                var newDeduction = new PayrollDeductionModel
                {
                    id = deductionId,
                    dependantNumber = Convert.ToInt32(dependantNumber),
                    formulaType = formulaType,
                    apiUrl = apiUrl,
                    apiMethod = apiMethod,
                    param1Value = param1Value,
                    param2Value = param2Value,
                    param3Value = param3Value,
                    param1Key = param1Key,
                    param2Key = param2Key,
                    param3Key = param3Key,
                    header1Value = header1Value,
                    header1Key = header1Key,
                    resultAmount = 0
                };
                payrollEmployees[payrollEmployeesIndex].deductions.Add(newDeduction);
            }
            return payrollEmployees;
        }

        private List<PayrollEmployeeModel> addPreviousComputedSalary(List<PayrollEmployeeModel> payrollEmployees
            , int payrollEmployeesIndex, DataRow dataRow)
        {
            var payrollId = utilityRepository.ConvertDatabaseValueToString(dataRow["payrollId"]);
            if (payrollId != "")
            {
                var previousSalary = utilityRepository.ConvertDatabaseValueToString(dataRow["previousComputedGrossSalary"]);
                var startDate = utilityRepository.ConvertDatabaseValueToString(dataRow["payrollStartDate"]);
                var newPreviousComputedGrossSalary = new PayrollPreviousComputedGrossSalary
                {
                    amount = Convert.ToDouble(previousSalary),
                    startDate = DateOnly.FromDateTime(Convert.ToDateTime(startDate))
                };
                payrollEmployees[payrollEmployeesIndex].previousComputedGrossSalaries.Add(newPreviousComputedGrossSalary);
            }
            return payrollEmployees;
        }

        private void checkDataTableCorrectness(DataTable dataTable)
        {
            if (dataTable.Rows.Count <= 0)
            {
                throw new Exception("Payroll employee table could not be extracted.");
            }
        }

        private SqlCommand createPayrollEmployeeTableCommand(string employerId, DateOnly startDate, DateOnly endDate)
        {
            var query = createPayrollTableQuery();
            var command = new SqlCommand(query, connectionRepository.connection);
            command.Parameters.AddWithValue("@employerId", employerId);
            command.Parameters.AddWithValue("@startDate", startDate);
            command.Parameters.AddWithValue("@endDate", endDate);
            command.Parameters.AddWithValue("@endDateMinusOneMonth", startDate.AddMonths(MONTHS_TO_SUBSTRACT));
            return command;
        }

        private string createPayrollTableQuery()
        {
            var query = "SELECT * FROM "
            + "( "
            + "SELECT "
            + "p.id id, p.fechaNacimiento birthDate, "
            + "pf.genero gender, "
            + "e.fechaContratacion hiringDate, "
            + "c.salarioBruto salary, c.tipoContrato hiringType, "
            + "h.fecha hoursDate, h.horasTrabajadas hoursNumber, "
            + "j.nombreAsociacion companyAssociation, "
            + "bp.cantidadDependientes dependantNumber, "
            + "d.id deductionId, f.tipoFormula formulaType, f.urlAPI apiUrl, f.paramUno param1Value, "
            + "f.paramDos param2Value, f.paramTres param3Value, "
            + "a.paramUnoClave param1Key, a.paramDosClave param2Key, "
            + "a.paramTresClave param3Key, a.metodo apiMethod, "
            + "a.headerUnoValor header1Value, a.headerUnoValor header1Key, "
            + "dp.salarioBruto previousComputedGrossSalary, "
            + "pla.id payrollId, pla.estado payrollState, pla.fechaInicio payrollStartDate "
            + "FROM Persona p "
            + "INNER JOIN Empleado e on e.idPersonaFisica = p.id "
            + "INNER JOIN PersonaFisica pf on pf.id = p.id "
            + "INNER JOIN Contrato as c on c.idEmpleado = e.idPersonaFisica "
            + "LEFT JOIN Horas as h on h.idEmpleado = e.idPersonaFisica "
            + "and h.[fecha] = ( "
            + "     SELECT fechaHoras "
            + "     FROM function_getEmployeeCurrentHours(e.[idPersonaFisica], @startDate, @endDate) "
            + "     ) "
            + "INNER JOIN Empleador o on o.idPersonaFisica = e.idEmpleadorContratador "
            + "INNER JOIN PersonaJuridica j on j.id = o.idPersonaJuridica "
            + "LEFT JOIN BeneficioPorEmpleado bp on bp.idEmpleado = p.id "
            + "LEFT JOIN Beneficio b on b.id = bp.idBeneficio "
            + "LEFT JOIN Deduccion d on d.idBeneficio = b.id "
            + "LEFT JOIN Formula f on f.id = d.idFormula "
            + "LEFT JOIN ApiExterna a on a.idFormula = f.id "
            + "LEFT JOIN DetallePago dp on dp.idEmpleado = e.idPersonaFisica "
            + "LEFT JOIN Planilla pla on pla.id = dp.idPlanilla "
            + "WHERE e.idEmpleadorContratador = @employerId and e.fechaDespido is null "
            + ") payroll "
            + "WHERE(payroll.payrollState = 'completado' or payroll.payrollState is NULL) AND "
            + "(payroll.payrollStartDate > @endDateMinusOneMonth or payroll.payrollStartDate is NULL) AND "
            + "(payroll.hiringDate <= @endDate) "
            + "ORDER BY payroll.id, payroll.deductionId, payroll.payrollStartDate";
            return query;
        }
    }
}
