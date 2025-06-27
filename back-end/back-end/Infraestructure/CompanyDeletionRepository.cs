using System.Data;
using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
    public class CompanyDeletionRepository : ICompanyDeletionRepository
    {
        private readonly AbstractConnectionRepository _connectionRepository;
        private readonly IUtilityRepository _utilityRepository;

        public CompanyDeletionRepository(AbstractConnectionRepository 
            connectionRepository, IUtilityRepository utilityRepository)
        {
            _connectionRepository = connectionRepository;
            _utilityRepository = utilityRepository;
        }

        public string getEmployerEmail(string companyName)
        {
            var command = getEmployerEmailCommand(companyName);
            var dataTable = _connectionRepository.ExecuteQuery(command);
            var employerEmail = getEmployerEmailFromDataTable(dataTable);
            return employerEmail;
        }

        public List<string> getEmployeesEmail(string companyName)
        {
            var command = getEmployeesEmailCommand(companyName);
            var dataTable = _connectionRepository.ExecuteQuery(command);
            var employeesEmail = getEmployeesEmailFromDataTable(dataTable);
            return employeesEmail;
        }

        public void deleteCompany(string employerEmail)
        {
            var command = getDeleteCompanyCommand(employerEmail);
            _connectionRepository.ExecuteCommand(command);
        }

        private SqlCommand getEmployerEmailCommand(string companyName)
        {
            var query = getEmployerEmailQuery();
            var command = new SqlCommand(query, _connectionRepository.connection);
            command.Parameters.AddWithValue("@companyName", companyName);
            return command;
        }

        private string getEmployerEmailQuery()
        {
            var query = @"
                select p.correoElectronico email
                from PersonaJuridica j
                inner join Empleador o on o.idPersonaJuridica = j.id
                inner join Persona p on p.id = o.idPersonaFisica
                where j.razonSocial = @companyName;";
            return query;
        }

        private string getEmployerEmailFromDataTable(DataTable dataTable)
        {
            checkDataTableCorrectness(dataTable);
            var dataRow = dataTable.Rows[0];
            var employerEmail = _utilityRepository.ConvertDatabaseValueToString(dataRow["email"]);
            return employerEmail;
        }

        private void checkDataTableCorrectness(DataTable dataTable)
        {
            if (dataTable.Rows.Count <= 0)
            {
                throw new Exception("CompanyDeletionRepository: The query did not return values.");
            }
            if (dataTable.Rows.Count > 1)
            {
                throw new Exception("CompanyDeletionRepository: The query returned more values than expected.");
            }
        }

        private SqlCommand getEmployeesEmailCommand(string companyName)
        {
            var query = getEmployeesEmailQuery();
            var command = new SqlCommand(query, _connectionRepository.connection);
            command.Parameters.AddWithValue("@companyName", companyName);
            return command;
        }

        private string getEmployeesEmailQuery()
        {
            var query = @"
                select p.correoElectronico email
                from PersonaJuridica j
                inner join Empleador o on o.idPersonaJuridica = j.id
                inner join Empleado e on e.idEmpleadorContratador = o.idPersonaFisica
                inner join Persona p on p.id = e.idPersonaFisica
                where j.razonSocial = @companyName;";
            return query;
        }

        private List<string> getEmployeesEmailFromDataTable(DataTable dataTable)
        {
            var employeesEmail = new List<string>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var employeeEmail = _utilityRepository.ConvertDatabaseValueToString(dataRow["email"]);
                employeesEmail.Add(employeeEmail);
            } 
            return employeesEmail;
        }

        private SqlCommand getDeleteCompanyCommand(string employerEmail)
        {
            var storedProcedure = "sp_deleteCompany";
            var command = new SqlCommand(storedProcedure, _connectionRepository.connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@mailEmployer", employerEmail);
            return command;
        }
    }
}
