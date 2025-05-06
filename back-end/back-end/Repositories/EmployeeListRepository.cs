using back_end.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace back_end.Repositories
{
  public class EmployeeListRepository
  {
    private SqlConnection _connection;
    private string _connectionRoute;
    public EmployeeListRepository()
    {
      var builder = WebApplication.CreateBuilder();
      _connectionRoute =
        builder.Configuration.GetConnectionString("InfinipayDBContext");
      _connection = new SqlConnection(_connectionRoute);
    }
    private DataTable getQueryTable(string query)
    {
      var queryCommand = new SqlCommand(query, _connection);
      var tableAdapter = new SqlDataAdapter(queryCommand);
      var queryTable = new DataTable();
      _connection.Open();
      tableAdapter.Fill(queryTable);
      _connection.Close();
      return queryTable;
    }

    public List<EmployeeListModel> obtainEmployeeInfo(string logguedId)
    {
      var dataTablePerson = obtainPersonInfo();
      var dataTableAddress = obtainAddress();
      var dataTableNatural = obtainNaturalPersonInfo();
      var dataTableEmployee = obtainEmployeeDetails(logguedId);
      var result = new List<EmployeeListModel>();
      foreach (var person in dataTablePerson)
      {
        var address = dataTableAddress.FirstOrDefault(d => d.id == person.id);
        var natural = dataTableNatural.FirstOrDefault(n => n.id == person.id);
        var employee = dataTableEmployee.FirstOrDefault(e => e.id == person.id);
        if (address != null && natural != null && employee != null)
        {
          result.Add(new EmployeeListModel
          {
            id = person.id,
            email = person.email,
            identification = person.identification,
            phoneNumber = person.phoneNumber,
            province = address.province,
            canton = address.canton,
            district = address.district,
            otherSigns = address.otherSigns,
            firstName = natural.firstName,
            secondName = natural.secondName,
            firstLastName = natural.firstLastName,
            secondLastName = natural.secondLastName,
            completeName = natural.completeName,
            role = employee.role,
            hireDate = employee.hireDate
          });
        }
      }
      return result;
    }
    public List<EmployeeListModel> obtainPersonInfo()
    {
      var persons = new List<EmployeeListModel>();
      string query = @"
        SELECT 
            id, correoElectronico, identificacion, numeroTelefono 
        FROM Persona";
      DataTable table = getQueryTable(query);
      foreach (DataRow rows in table.Rows)
      {
        persons.Add(new EmployeeListModel
        {
          id = Guid.Parse(rows["id"].ToString()),
          email = Convert.ToString(rows["correoElectronico"]),
          identification = Convert.ToString(rows["identificacion"]),
          phoneNumber = Convert.ToString(rows["numeroTelefono"])
        });
      }
      return persons;
    }
    public List<EmployeeListModel> obtainAddress()
    {
      var addresses = new List<EmployeeListModel>();
      string query = @"
        SELECT 
           idPersona, provincia, canton, distrito, otrasSenas 
        FROM Direccion";
      DataTable table = getQueryTable(query);
      foreach (DataRow rows in table.Rows)
      {
        addresses.Add(new EmployeeListModel
        {
          id = Guid.Parse(rows["idPersona"].ToString()),
          province = Convert.ToString(rows["provincia"]),
          canton = Convert.ToString(rows["canton"]),
          district = Convert.ToString(rows["distrito"]),
          otherSigns = Convert.ToString(rows["otrasSenas"])
        });
      }
      return addresses;
    }
    public List<EmployeeListModel> obtainNaturalPersonInfo()
    {
      var naturalPersons = new List<EmployeeListModel>();
      string query = @"
        SELECT 
           id, primerNombre, segundoNombre, primerApellido, segundoApellido  
        FROM PersonaFisica";
      DataTable table = getQueryTable(query);
      foreach (DataRow rows in table.Rows)
      {
        string firstName = Convert.ToString(rows["primerNombre"]);
        string secondName = Convert.ToString(rows["segundoNombre"]);
        string firstLastName = Convert.ToString(rows["primerApellido"]);
        string secondLastName = Convert.ToString(rows["segundoApellido"]);
        string completeName
           = string.Join(" ", new[] { firstName, secondName, firstLastName
           , secondLastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
        naturalPersons.Add(new EmployeeListModel
        {
          id = Guid.Parse(rows["id"].ToString()),
          completeName = completeName,
          firstName = firstName,
          secondName = secondName,
          firstLastName = firstLastName,
          secondLastName = secondLastName
        });
      }
      return naturalPersons;
    }
    public List<EmployeeListModel> obtainEmployeeDetails(string logguedId)
    {
      var employeeDetails = new List<EmployeeListModel>();
      string query = $@"
        SELECT 
           idPersonaFisica, rol, fechaContratacion, observaciones
        FROM Empleado
        WHERE idEmpleadorContratador = '{logguedId}';";
      DataTable table = getQueryTable(query);
      foreach (DataRow rows in table.Rows)
      {
        employeeDetails.Add(new EmployeeListModel
        {
          id = Guid.Parse(rows["idPersonaFisica"].ToString()),
          role = Convert.ToString(rows["rol"]),
          hireDate = Convert.ToString(rows["fechaContratacion"]),
          observations = Convert.ToString(rows["observaciones"])
        });
      }
      return employeeDetails;
    }
  }
}