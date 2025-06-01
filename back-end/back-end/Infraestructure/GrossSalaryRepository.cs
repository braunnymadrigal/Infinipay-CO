using System.Data;
using System.Diagnostics.Contracts;
using System.Reflection;
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
            var hiringDate = utilityRepository.ConvertDatabaseValueToString(dataRow["fechaContratacion"]);
            var grossSalary = utilityRepository.ConvertDatabaseValueToString(dataRow["salarioBruto"]);
            var hiringType = utilityRepository.ConvertDatabaseValueToString(dataRow["tipoContrato"]);
            var hoursDate = utilityRepository.ConvertDatabaseValueToString(dataRow["fechaHoras"]);
            var hoursWorked = utilityRepository.ConvertDatabaseValueToString(dataRow["horasTrabajadas"]);
            var actualHoursDate = hoursDate != "" ? DateOnly.FromDateTime(Convert.ToDateTime(hoursDate)) : DateOnly.MinValue;
            var actualHoursWorked = hoursWorked != "" ? Convert.ToInt32(hoursWorked) : 0;
            var actualGrossSalary = Convert.ToDouble(grossSalary);
            return new GrossSalaryModel {
                EmployeeId = employeeId, HiringDate = DateOnly.FromDateTime(Convert.ToDateTime(hiringDate)),
                ComputedGrossSalary = actualGrossSalary, GrossSalary = actualGrossSalary, HiringType = hiringType,
                HoursDate = actualHoursDate, HoursWorked = actualHoursWorked,
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

        private string CreatePayrollTableQuery()
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
            + "f.id formulaId, f.tipoFormula formulaType, f.urlAPI apiUrl, f.paramUno param1Value, "
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
            + "ORDER BY payroll.id, payroll.formulaId, payroll.payrollStartDate";
            return query;
        }
    }
}
