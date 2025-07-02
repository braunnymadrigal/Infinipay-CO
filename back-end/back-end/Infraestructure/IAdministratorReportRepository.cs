using back_end.Domain;

namespace back_end.Infraestructure
{
  public interface IAdministratorReportRepository
  {
    public List<PayrollAdministratorModel> getAllCompaniesPayroll(DateOnly
      startDate, DateOnly endDate);
  }
}
