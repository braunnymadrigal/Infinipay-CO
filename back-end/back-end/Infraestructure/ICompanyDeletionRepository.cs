namespace back_end.Infraestructure
{
    public interface ICompanyDeletionRepository
    {
        string getEmployerEmail(string companyName);
        List<string> getEmployeesEmail(string companyName);
        void deleteCompany(string employerEmail);
    }
}
