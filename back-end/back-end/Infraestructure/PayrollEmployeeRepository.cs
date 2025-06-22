using System.Data;
using back_end.Domain;
using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
    public class PayrollEmployeeRepository : IPayrollEmployeeRepository
    {
        private const int FIRST_DAY_OF_ANY_MONTH = 1;
        private const int PAYROLL_EMPLOYEE_LIST_INITIAL_INDEX = -1;

        private readonly AbstractConnectionRepository _connectionRepository;
        private readonly IUtilityRepository _utilityRepository;

        public PayrollEmployeeRepository(AbstractConnectionRepository connectionRepository
            , IUtilityRepository utilityRepository)
        {
            _connectionRepository = connectionRepository;
            _utilityRepository = utilityRepository;
        }

        public List<PayrollEmployeeModel> getPayrollEmployees(PayrollEmployerModel payrollEmployer)
        {
            var command = createPayrollEmployeeTableCommand(payrollEmployer);
            var dataTable = _connectionRepository.ExecuteQuery(command);
            var payrollEmployees = transformDataTableIntoPayrollEmployeeList(dataTable);
            return payrollEmployees;
        }

        private List<PayrollEmployeeModel> transformDataTableIntoPayrollEmployeeList(DataTable dataTable)
        {
            checkDataTableCorrectness(dataTable);
            var payrollEmployees = new List<PayrollEmployeeModel>();
            var payrollEmployeesIndex = PAYROLL_EMPLOYEE_LIST_INITIAL_INDEX;
            var previousId = "";
            var deductionIds = new HashSet<string>();
            var payrollIds = new HashSet<string>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var id = _utilityRepository.ConvertDatabaseValueToString(dataRow["id"]);
                var deductionId = _utilityRepository.ConvertDatabaseValueToString(dataRow["deductionId"]);
                var payrollId = _utilityRepository.ConvertDatabaseValueToString(dataRow["payrollId"]);
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
            var id = _utilityRepository.ConvertDatabaseValueToString(dataRow["id"]);
            var birthDate = _utilityRepository.ConvertDatabaseValueToString(dataRow["birthDate"]);
            var gender = _utilityRepository.ConvertDatabaseValueToString(dataRow["gender"]);
            var name = _utilityRepository.ConvertDatabaseValueToString(dataRow["name"]);
            var hiringDate = _utilityRepository.ConvertDatabaseValueToString(dataRow["hiringDate"]);
            var rawGrossSalary = _utilityRepository.ConvertDatabaseValueToString(dataRow["salary"]);
            var hiringType = _utilityRepository.ConvertDatabaseValueToString(dataRow["hiringType"]);
            var companyAssociaton = _utilityRepository.ConvertDatabaseValueToString(dataRow["companyAssociation"]);
            var newPayrollEmployee = new PayrollEmployeeModel
            {
                id = id,
                birthDate = DateOnly.FromDateTime(Convert.ToDateTime(birthDate)),
                gender = gender,
                name = name,
                hiringDate = DateOnly.FromDateTime(Convert.ToDateTime(hiringDate)),
                rawGrossSalary = Convert.ToDouble(rawGrossSalary),
                hiringType = hiringType,
                companyAssociation = companyAssociaton,
            };
            payrollEmployees.Add(newPayrollEmployee);
            return payrollEmployees;
        }

        private List<PayrollEmployeeModel> addPayrollDeductionModel(List<PayrollEmployeeModel> payrollEmployees
            , int payrollEmployeesIndex, DataRow dataRow)
        {
            var deductionId = _utilityRepository.ConvertDatabaseValueToString(dataRow["deductionId"]);
            if (deductionId != "")
            {
                var dependantNumber = _utilityRepository.ConvertDatabaseValueToString(dataRow["dependantNumber"]);
                var formulaType = _utilityRepository.ConvertDatabaseValueToString(dataRow["formulaType"]);
                var apiUrl = _utilityRepository.ConvertDatabaseValueToString(dataRow["apiUrl"]);
                var apiMethod = _utilityRepository.ConvertDatabaseValueToString(dataRow["apiMethod"]);
                var param1Value = _utilityRepository.ConvertDatabaseValueToString(dataRow["param1Value"]);
                var param2Value = _utilityRepository.ConvertDatabaseValueToString(dataRow["param2Value"]);
                var param3Value = _utilityRepository.ConvertDatabaseValueToString(dataRow["param3Value"]);
                var param1Key = _utilityRepository.ConvertDatabaseValueToString(dataRow["param1Key"]);
                var param2Key = _utilityRepository.ConvertDatabaseValueToString(dataRow["param2Key"]);
                var param3Key = _utilityRepository.ConvertDatabaseValueToString(dataRow["param3Key"]);
                var header1Value = _utilityRepository.ConvertDatabaseValueToString(dataRow["header1Value"]);
                var header1Key = _utilityRepository.ConvertDatabaseValueToString(dataRow["header1Key"]);
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
                };
                payrollEmployees[payrollEmployeesIndex].deductions.Add(newDeduction);
            }
            return payrollEmployees;
        }

        private List<PayrollEmployeeModel> addPreviousComputedSalary(List<PayrollEmployeeModel> payrollEmployees
            , int payrollEmployeesIndex, DataRow dataRow)
        {
            var payrollId = _utilityRepository.ConvertDatabaseValueToString(dataRow["payrollId"]);
            if (payrollId != "")
            {
                var previousSalary = _utilityRepository.ConvertDatabaseValueToString(dataRow["previousComputedGrossSalary"]);
                payrollEmployees[payrollEmployeesIndex].previousComputedGrossSalaries.Add(Convert.ToDouble(previousSalary));
            }
            return payrollEmployees;
        }

        private void checkDataTableCorrectness(DataTable dataTable)
        {
            if (dataTable.Rows.Count <= 0)
            {
                throw new Exception("PayrollEmployeeRepository: The query did not return values.");
            }
        }

        private SqlCommand createPayrollEmployeeTableCommand(PayrollEmployerModel payrollEmployer)
        {
            var query = createPayrollTableQuery();
            var command = new SqlCommand(query, _connectionRepository.connection);
            command.Parameters.AddWithValue("@employerId", payrollEmployer.id);
            command.Parameters.AddWithValue("@endDate", payrollEmployer.endDate);
            var firstDayOfMonth = new DateOnly(payrollEmployer.endDate.Year, 
                payrollEmployer.endDate.Month, FIRST_DAY_OF_ANY_MONTH);
            command.Parameters.AddWithValue("@firstDayOfMonth", firstDayOfMonth);
            return command;
        }

        private string createPayrollTableQuery()
        {
            var query = @"
                SELECT
	                p.id id, p.fechaNacimiento birthDate,
	                pf.genero gender,
	                CONCAT_WS(
		                ' ', pf.primerNombre, pf.segundoNombre, pf.primerApellido,
                        pf.segundoApellido
	                ) [name],
	                e.fechaContratacion hiringDate,
	                c.salarioBruto salary, c.tipoContrato hiringType,
	                j.nombreAsociacion companyAssociation,
	                bp.cantidadDependientes dependantNumber,
	                d.id deductionId, d.nombre deductionName,
                    f.tipoFormula formulaType, f.urlAPI apiUrl, f.paramUno param1Value,
	                f.paramDos param2Value, f.paramTres param3Value,
	                a.paramUnoClave param1Key, a.paramDosClave param2Key,
	                a.paramTresClave param3Key, a.metodo apiMethod,
	                a.headerUnoValor header1Value, a.headerUnoClave header1Key,
	                dp.salarioBruto previousComputedGrossSalary,
	                CASE
		                WHEN pla.fechaInicio < @firstDayOfMonth THEN null
		                ELSE pla.id
	                END AS payrollId
                FROM Persona p
                INNER JOIN Empleado e on e.idPersonaFisica = p.id
                INNER JOIN PersonaFisica pf on pf.id = p.id
                INNER JOIN Contrato as c on c.idEmpleado = e.idPersonaFisica
                INNER JOIN Empleador o on o.idPersonaFisica = e.idEmpleadorContratador
                INNER JOIN PersonaJuridica j on j.id = o.idPersonaJuridica
                LEFT JOIN BeneficioPorEmpleado bp on bp.idEmpleado = p.id
                LEFT JOIN Beneficio b on b.id = bp.idBeneficio
                LEFT JOIN Deduccion d on d.idBeneficio = b.id
                LEFT JOIN Formula f on f.id = d.idFormula
                LEFT JOIN ApiExterna a on a.idFormula = f.id
                LEFT JOIN DetallePago dp on dp.idEmpleado = e.idPersonaFisica
                LEFT JOIN Planilla pla on pla.id = dp.idPlanilla 
		                  and (pla.estado is null or pla.estado = 'completado') 

                WHERE 
	                e.idEmpleadorContratador = @employerId and 
	                e.fechaDespido is null and
	                e.fechaContratacion <= @endDate

                ORDER BY p.id, d.id, pla.id;";
            return query;
        }
    }
}
