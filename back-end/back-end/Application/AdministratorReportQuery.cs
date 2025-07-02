using back_end.Domain;
using back_end.Infraestructure;

namespace back_end.Application
{
  public class AdministratorReportQuery : IAdministratorReportQuery
  {
    private readonly IAdministratorReportRepository
      administratorReportRepository;
    public AdministratorReportQuery(IAdministratorReportRepository
            administratorReportRepository)
    {
      this.administratorReportRepository = administratorReportRepository;
    }

    public List<PayrollAdministratorModel> getAllCompaniesPayroll(
      DateOnly startDate, DateOnly endDate)
    {
      try
      {
        var errorMsg = string.Empty;

        var crTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central" +
          " America Standard Time");
        DateTime localTimeCr
          = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, crTimeZone);
        DateOnly todayCr = DateOnly.FromDateTime(localTimeCr);

        if (startDate > todayCr) errorMsg
            += "Fecha inicial invalida. ";

        if (endDate > todayCr) errorMsg
            += "Fecha final invalida. ";

        if (errorMsg != string.Empty) throw new Exception(errorMsg);

        if (startDate > endDate)
        {
          errorMsg += "Fecha inicial debe ser menor o igual a la final. ";
          throw new Exception(errorMsg);
        }

        return
          administratorReportRepository.getAllCompaniesPayroll(
            startDate, endDate);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }
  }
}
