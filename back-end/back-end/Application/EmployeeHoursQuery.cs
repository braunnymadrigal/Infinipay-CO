using back_end.Domain;
using back_end.Infraestructure;
using Microsoft.IdentityModel.Tokens;

namespace back_end.Application
{
  public class EmployeeHoursQuery : IEmployeeHoursQuery
  {
    private readonly IEmployeeHoursRepository employeeHoursRepository;
    public EmployeeHoursQuery(IEmployeeHoursRepository
      employeeHoursRepository)
    {
      this.employeeHoursRepository = employeeHoursRepository;
    }
    public EmployeeHoursModel getEmployeeHoursContract(string loggedUserId)
    {
      if (string.IsNullOrEmpty(loggedUserId))
      {
        throw new ArgumentNullException(nameof(loggedUserId));
      }

      EmployeeHoursModel employeeHoursModel
        = employeeHoursRepository.getEmployeeHoursContract(loggedUserId);

      if (employeeHoursModel == null)
      {
        throw new InvalidDataException(
          "No se puedo obtener detalles del contrato.");
      }

      return employeeHoursModel;
    }

    public List<HoursModel> getEmployeeHoursList(string loggedUserId,
      DateOnly startDate, DateOnly endDate)
    {
      if (string.IsNullOrEmpty(loggedUserId))
      {
        throw new ArgumentNullException(nameof(loggedUserId));
      }

      if (startDate >= endDate)
      {
        throw new InvalidDataException("Fecha de inicio mayor que fecha" +
          "de fin");
      }

      List<HoursModel> hours = employeeHoursRepository.getEmployeeHoursList(
        loggedUserId, startDate, endDate);
      if (hours == null)
      {
        throw new InvalidDataException("Horas no obtenidas");
      }

      return hours;
    }
  }
}
