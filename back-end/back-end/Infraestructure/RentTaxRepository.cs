using System;
using System.Data;
using System.Text.RegularExpressions;
using back_end.Domain;
namespace back_end.Infraestructure

{
  public class RentTaxRepository
  {
    private readonly DataBaseConnectionRepository _dataBaseConnection;

    public RentTaxRepository(DataBaseConnectionRepository
      dataBaseConnection)
    {
      _dataBaseConnection = dataBaseConnection;
    }

    public (string contractType, decimal grossSalary)
      GetEmployeeSalaryInfo(string employeeId)
    {
      // TODO: Obtener la fecha semanal de las horas trabajadas y las horas
      // hacer un JOIN
      string query = $@"
        SELECT salarioBruto, tipoContrato
        FROM Contrato
        WHERE idEmpleado = '{employeeId}'";

      DataTable table = _dataBaseConnection.ExecuteQuery(query);

      if (table.Rows.Count == 0)
        throw new Exception("Empleado no encontrado o sin salario asignado.");

      var contractType =
        table.Rows[0]["tipoContrato"].ToString();
      var grossSalary =
        Convert.ToDecimal(table.Rows[0]["salarioBruto"]);

      return (contractType, grossSalary);
    }
  }
}
