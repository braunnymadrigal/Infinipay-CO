using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using back_end.Infraestructure;
using back_end.Domain;

namespace back_end.Application
{
  public class EmployeeQuery : IEmployeeQuery
  {
    private readonly EmployeeRepository _employeeRepository;

    public EmployeeQuery()
    {
      _employeeRepository = new EmployeeRepository();
    }

    public EmployeeModel GetEmployee(Guid id)
    {
        if (id == Guid.Empty)
        {
          throw new ArgumentException("Invalid employee ID.");
        }
        try
        {
          return _employeeRepository.GetEmployeeById(id);
        }
        catch (Exception ex)
        {
          throw new Exception("Error retrieving employee: " + ex.Message);
        }
    }
  }
}