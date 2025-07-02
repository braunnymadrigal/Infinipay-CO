using back_end.Domain;

namespace back_end.Application
{
  public interface IAdministratorReportQuery
  {
    public List<PayrollAdministratorModel> getAllCompaniesPayroll(
      DateOnly startDate, DateOnly endDate);
  }
}
