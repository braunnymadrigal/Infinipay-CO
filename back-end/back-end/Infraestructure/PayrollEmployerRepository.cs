using System.ComponentModel.Design;
using System.Data;
using back_end.Domain;
using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
    public class PayrollEmployerRepository : IPayrollEmployerRepository
    {
        private readonly AbstractConnectionRepository _connectionRepository;
        private readonly IUtilityRepository _utilityRepository;

        public PayrollEmployerRepository(AbstractConnectionRepository connectionRepository
            , IUtilityRepository utilityRepository)
        {
            _connectionRepository = connectionRepository;
            _utilityRepository = utilityRepository;
        }

        public PayrollEmployerModel getPayrollEmployer(PayrollEmployerModel payrollEmployer)
        {
            var command = createGetPayrollEmployerCommand(payrollEmployer.id);
            var dataTable = _connectionRepository.ExecuteQuery(command);
            payrollEmployer = transformGetPayrollEmployerDataTable(dataTable, payrollEmployer);
            return payrollEmployer;
        }

        public DateOnly getDateOfLatestPayroll(string companyId)
        {
            var command = createGetDateOfLatestPayrollCommand(companyId);
            var dataTable = _connectionRepository.ExecuteQuery(command);
            var latestDate = transformGetDateOfLatestPayrollDataTable(dataTable);
            return latestDate;
        }

        private SqlCommand createGetPayrollEmployerCommand(string id)
        {
            var query = createGetPayrollEmployerQuery();
            var command = new SqlCommand(query, _connectionRepository.connection);
            command.Parameters.AddWithValue("@employerId", id);
            return command;
        }

        private SqlCommand createGetDateOfLatestPayrollCommand(string companyId)
        {
            var query = createGetDateOfLatestPayrollQuery();
            var command = new SqlCommand(query, _connectionRepository.connection);
            command.Parameters.AddWithValue("@companyId", companyId);
            return command;
        }

        private string createGetPayrollEmployerQuery()
        {
            var query = @"
                SELECT 
	                j.id companyId, j.tipoPago paymentType
                FROM 
	                Empleador e
                INNER JOIN 
	                PersonaJuridica j on e.idPersonaJuridica = j.id
                WHERE 
	                e.idPersonaFisica = @employerId;";
            return query;
        }

        private string createGetDateOfLatestPayrollQuery()
        {
            var query = @"
                SELECT 
	                TOP 1
	                p.fechaFin latestEndDate
                FROM 
	                Planilla p
                WHERE
	                p.idPersonaJuridica = @companyId and
	                p.estado = 'completado'
                ORDER BY
	                p.fechaFin DESC;";
            return query;
        }

        private PayrollEmployerModel transformGetPayrollEmployerDataTable(DataTable dataTable, 
            PayrollEmployerModel payrollEmployer)
        {
            checkDataTableCorrectness(dataTable);
            var dataRow = dataTable.Rows[0];
            var companyId = _utilityRepository.ConvertDatabaseValueToString(dataRow["companyId"]);
            var paymentType = _utilityRepository.ConvertDatabaseValueToString(dataRow["paymentType"]);
            payrollEmployer.companyId = companyId;
            payrollEmployer.paymentType = paymentType;
            return payrollEmployer;
        }

        private DateOnly transformGetDateOfLatestPayrollDataTable(DataTable dataTable)
        {
            var latestEndDate = DateOnly.MinValue;
            if (dataTable.Rows.Count > 0)
            {
                var dataRow = dataTable.Rows[0];
                var returnedDate = _utilityRepository.ConvertDatabaseValueToString(dataRow["latestEndDate"]);
                if (returnedDate != "")
                {
                    latestEndDate = DateOnly.FromDateTime(Convert.ToDateTime(returnedDate));
                }
            }
            return latestEndDate;
        }

        private void checkDataTableCorrectness(DataTable dataTable)
        {
            if (dataTable.Rows.Count <= 0)
            {
                throw new Exception("PayrollEmployerRepository: The query did not return values.");
            }
            if (dataTable.Rows.Count > 1)
            {
                throw new Exception("PayrollEmployerRepository: The query returned more values than expected.");
            }
        }
    }
}
