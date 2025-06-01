using back_end.Domain;
using back_end.Infraestructure;

namespace back_end.Application
{
  public class EmployeeHoursCommand : IEmployeeHoursCommand
  {
    private readonly IEmployeeHoursRepository employeeHoursRepository;
    public EmployeeHoursCommand(IEmployeeHoursRepository
      employeeHoursRepository)
    {
      this.employeeHoursRepository = employeeHoursRepository;
    }

    public bool RegisterEmployeeHours(string loggedUserId
      , List<HoursModel> employeeHoursWorked)
    {
      try
      {
        if (string.IsNullOrEmpty(loggedUserId))
        {
          throw new ArgumentNullException(nameof(loggedUserId));
        }

        if (employeeHoursWorked == null || employeeHoursWorked.Count == 0)
        {
          throw new InvalidDataException("No hay horas por registrar");
        }

        for (int i = 0; i < employeeHoursWorked.Count; i++)
        {
          if (employeeHoursWorked[i] == null)
          {
            throw new InvalidDataException("Horas registradas invalidas");
          }

          if (employeeHoursWorked[i].date > DateOnly.FromDateTime(DateTime.Now))
          {
            throw new InvalidDataException("Fecha de registro invalido");
          }

          if (employeeHoursWorked[i].hoursWorked <= 0
            || employeeHoursWorked[i].hoursWorked > 9)
          {
            throw new InvalidDataException("Cantidad de horas" +
              " registradas invalidas");
          }
        }

        return employeeHoursRepository.RegisterEmployeeHours(loggedUserId
          , employeeHoursWorked);
      }
      catch (Exception ex)
      {
        throw new Exception("Error en datos de horas: "
          + ex.Message, ex);
      }
    }
  }
}
