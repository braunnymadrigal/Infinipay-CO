using System.Data;
using back_end.Domain;
using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
    public class PayrollEmployeeRepository : IPayrollEmployeeRepository
    {
        private const int MONTHS_TO_SUBSTRACT = -1;

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
            List<PayrollEmployeeModel> payrollEmployees = new List<PayrollEmployeeModel>();

            var previousId = "";
            var previousDeductionId = "";
            //var previousPayrollStartDate = "";
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var id = utilityRepository.ConvertDatabaseValueToString(dataRow["id"]);
                var deductionId = utilityRepository.ConvertDatabaseValueToString(dataRow["deductionId"]);
                //var payrollStartDate = utilityRepository.ConvertDatabaseValueToString(dataRow["payrollStartDate"]);

                if (previousId != id)
                {
                    // addPayrollEmployeeModel(payrollEmployees, dataRow);
                }
                else
                {
                    if (deductionId != previousDeductionId)
                    {
                        // ddPayrollDeductionModel(payrollEmployees[i], dataRow);
                    }
                    else
                    {
                        // AddPreviousComputedGrossSalary(payrollEmployess[i], dataRow);
                    }
                }
                //var currentPayrollEmployee = transformDataRowInPayrollEmployeeModel(dataRow);
                //payrollEmployees.Add(currentPayrollEmployee);
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

        private PayrollEmployeeModel transformDataRowInPayrollEmployeeModel(DataRow dataRow)
        {
            var id = utilityRepository.ConvertDatabaseValueToString(dataRow["id"]);
            var birthDate = utilityRepository.ConvertDatabaseValueToString(dataRow["birthDate"]);
            var gender = utilityRepository.ConvertDatabaseValueToString(dataRow["gender"]);
            var salary = utilityRepository.ConvertDatabaseValueToString(dataRow["salary"]);
            var hiringType = utilityRepository.ConvertDatabaseValueToString(dataRow["hiringType"]);
            var hoursDate = utilityRepository.ConvertDatabaseValueToString(dataRow["hoursDate"]);
            var hoursNumber = utilityRepository.ConvertDatabaseValueToString(dataRow["hoursNumber"]);
            var companyAssociaton = utilityRepository.ConvertDatabaseValueToString(dataRow["companyAssociation"]);

            var deductionId = utilityRepository.ConvertDatabaseValueToString(dataRow["deductionId"]);
            var payrollStartDate = utilityRepository.ConvertDatabaseValueToString(dataRow["payrollStartDate"]);
            
            var benefitDependantNumber = utilityRepository.ConvertDatabaseValueToString(dataRow["benefitDependantNumber"]);
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

            var previousComputedGrossSalary = utilityRepository.ConvertDatabaseValueToString(dataRow["previousComputedGrossSalary"]);

            //var hiringDate = utilityRepository.ConvertDatabaseValueToString(dataRow["fechaContratacion"]);
            //var grossSalary = utilityRepository.ConvertDatabaseValueToString(dataRow["salarioBruto"]);
            //var hiringType = utilityRepository.ConvertDatabaseValueToString(dataRow["tipoContrato"]);
            //var hoursDate = utilityRepository.ConvertDatabaseValueToString(dataRow["fechaHoras"]);
            //var hoursWorked = utilityRepository.ConvertDatabaseValueToString(dataRow["horasTrabajadas"]);
            //var actualHoursDate = hoursDate != "" ? DateOnly.FromDateTime(Convert.ToDateTime(hoursDate)) : DateOnly.MinValue;
            //var actualHoursWorked = hoursWorked != "" ? Convert.ToInt32(hoursWorked) : 0;
            //var actualGrossSalary = Convert.ToDouble(grossSalary);
            //return new GrossSalaryModel
            //{
            //    EmployeeId = employeeId,
            //    HiringDate = DateOnly.FromDateTime(Convert.ToDateTime(hiringDate)),
            //    ComputedGrossSalary = actualGrossSalary,
            //    GrossSalary = actualGrossSalary,
            //    HiringType = hiringType,
            //    HoursDate = actualHoursDate,
            //    HoursWorked = actualHoursWorked,
            //};
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
            + "bp.cantidadDependientes benefitDependantNumber, "
            + "d.id deductionId, f.tipoFormula formulaType, f.urlAPI apiUrl, f.paramUno param1Value, "
            + "f.paramDos param2Value, f.paramTres param3Value, "
            + "a.paramUnoClave param1Key, a.paramDosClave param2Key, "
            + "a.paramTresClave param3Key, a.metodo apiMethod, "
            + "a.headerUnoValor header1Value, a.headerUnoValor header1Key, "
            + "dp.salarioBruto previousComputedGrossSalary, "
            + "pla.estado payrollState, pla.fechaInicio payrollStartDate "
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
            + "(payroll.payrollStartDate > @endDateMinusOneMonth or payroll.payrollStartDate is NULL) "
            + "ORDER BY payroll.id, payroll.deductionId, payroll.payrollStartDate";
            return query;
        }
    }
}
