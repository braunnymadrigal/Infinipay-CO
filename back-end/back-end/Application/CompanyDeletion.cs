using back_end.Infraestructure;

namespace back_end.Application
{
    public class CompanyDeletion : ICompanyDeletion
    {
        private readonly ICompanyDeletionRepository _companyDeletionRepository;

        public CompanyDeletion(ICompanyDeletionRepository companyDeletionRepository)
        {
            _companyDeletionRepository = companyDeletionRepository;
        }

        public List<string> deleteCompany(string companyName)
        {
            var employerEmail = _companyDeletionRepository.getEmployerEmail(companyName);
            var employeesEmail = _companyDeletionRepository.getEmployeesEmail(companyName);
            _companyDeletionRepository.deleteCompany(employerEmail);
            employeesEmail.Add(employerEmail);
            return employeesEmail;
        }
    }
}
