using back_end.Domain;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;

namespace back_end.Infraestructure
{
  public class EmployeeHoursRepository : IEmployeeHoursRepository
  {
    private readonly AbstractConnectionRepository connectionRepository;
    public EmployeeHoursRepository()
    {
      connectionRepository = new ConnectionRepository();
    }

    public EmployeeHoursModel GetEmployeeHoursContract(string loggedUserId)
    {
      EmployeeHoursModel employeeHoursModel = new EmployeeHoursModel();

      string query = @"
        SELECT 
	        c.reportaHoras,
	        c.tipoContrato
        FROM Contrato c
        WHERE c.idEmpleado = @loggedUserId;
      ";

      try
      {
        var command = new SqlCommand(query, connectionRepository.connection);
        command.Parameters.AddWithValue("@loggedUserId", loggedUserId);

        var dataTable = connectionRepository.ExecuteQuery(command);

        if (dataTable.Rows.Count == 0)
        {
          throw new Exception("Ningún dato retornado del usuario.");
        }

        var dataRow = dataTable.Rows[0];

        if (dataRow["tipoContrato"] == DBNull.Value)
        {
          throw new Exception("El tipo de contrato es requerido " +
            "pero no fue encontrado.");
        }

        if (dataRow["reportaHoras"] == DBNull.Value)
        {
          throw new Exception("El campo reportaHoras es requerido" +
            " pero no fue encontrado.");
        }

        employeeHoursModel.typeContract
          = Convert.ToString(dataRow["tipoContrato"]);
        employeeHoursModel.reportsHours
          = Convert.ToBoolean(dataRow["reportaHoras"]);
      }
      catch (Exception ex)
      {
        throw new Exception("Error al obtener datos del contrato: "
          + ex.Message, ex);
      }

      return employeeHoursModel;
    }

    public List<HoursModel> GetEmployeeHoursList(string loggedUserId
      , DateOnly startDate, DateOnly endDate)
    {

      List<HoursModel> hoursModel = new List<HoursModel>();

      string query = @"
        SELECT
          h.Fecha,
          h.horasTrabajadas,
          h.aprobadas,
          h.idSupervisor
        FROM Horas h
        WHERE idEmpleado = @loggedUserId 
          AND h.Fecha BETWEEN @startDate AND @endDate;
      ";

      try
      {
        var command = new SqlCommand(query, connectionRepository.connection);

        command.Parameters.AddWithValue("@loggedUserId", loggedUserId);

        command.Parameters.AddWithValue("@startDate"
          , startDate.ToDateTime(TimeOnly.MinValue));

        command.Parameters.AddWithValue("@endDate"
          , endDate.ToDateTime(TimeOnly.MinValue));

        var dataTable = connectionRepository.ExecuteQuery(command);
        if (dataTable.Rows.Count == 0)
        {
          return hoursModel;
        }

        foreach (DataRow row in dataTable.Rows)
        {
          hoursModel.Add(new HoursModel
          {
            date = DateOnly.FromDateTime((DateTime)row["Fecha"]),

            hoursWorked = (short)row["horasTrabajadas"],

            approved = row["aprobadas"] != DBNull.Value
              ? Convert.ToBoolean(row["aprobadas"]) : null,

            supervidorId = row["idSupervisor"] != DBNull.Value
              ? (Guid)row["idSupervisor"] : null
          });
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error al obtener datos del contrato: "
          + ex.Message, ex);
      }
      return hoursModel;
    }

    public bool RegisterEmployeeHours(string loggedUserId
      , List<HoursModel> employeeHoursWorked)
    {
      bool success = false;

      var command = new SqlCommand();
      command.Connection = connectionRepository.connection;

      var queryBuilder = new StringBuilder();

      queryBuilder.Append("INSERT INTO Horas(fecha, horasTrabajadas" +
        ", idEmpleado) VALUES ");

      for (int i = 0; i < employeeHoursWorked.Count; i++)
      {
        var row = employeeHoursWorked[i];
        string dateParam = $"@date{i}";
        string hoursParam = $"@hours{i}";
        string userParam = $"@user{i}";

        queryBuilder.Append($"({dateParam}, {hoursParam}, {userParam})");

        if (i < employeeHoursWorked.Count - 1)
          queryBuilder.Append(", ");

        command.Parameters.AddWithValue(dateParam
          , row.date.ToDateTime(TimeOnly.MinValue));
        command.Parameters.AddWithValue(hoursParam, row.hoursWorked);
        command.Parameters.AddWithValue(userParam, loggedUserId);
      }

      command.CommandText = queryBuilder.ToString();

      try
      {
        connectionRepository.ExecuteCommand(command);

      }
      catch (Exception ex)
      {
        throw new Exception("Error al insertar horas a la base de datos: "
          + ex.Message, ex);
      }
      finally
      {
        success = true;
      }

      return success;
    }
  }
}
