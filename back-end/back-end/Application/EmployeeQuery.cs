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

    public List<EmployeeModel> GetAllEmployees(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
          throw new ArgumentException("Invalid user ID.");
        }
        Console.WriteLine("Fetching employees for user ID: " + userId);
        try
        {
          var companyId = _employeeRepository.getCompanyId(userId);
          if (companyId == Guid.Empty)
          {
            throw new Exception("Company ID not found for the given user ID.");
          }
          return _employeeRepository.GetAllEmployees(companyId);
        }
        catch (Exception ex)
        {
          throw new Exception("Error retrieving employees: " + ex.Message);
        }
    }

  }
}