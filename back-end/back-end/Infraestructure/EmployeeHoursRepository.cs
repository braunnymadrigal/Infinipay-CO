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
    public EmployeeHoursRepository(AbstractConnectionRepository
      connectionRepository)
    {
      this.connectionRepository = connectionRepository;
    }

    private string getEmployeeHoursContractQuery()
    {
      return @"
        SELECT 
	        c.reportaHoras,
	        c.tipoContrato
        FROM Contrato c
        WHERE c.idEmpleado = @loggedUserId;
      ";
    }

    private void getEmployeeHoursContractFromTable(EmployeeHoursModel
      employeeHoursModel, DataTable tableResult)
    {
      var dataRow = tableResult.Rows[0];

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

    public EmployeeHoursModel getEmployeeHoursContract(string loggedUserId)
    {
      EmployeeHoursModel employeeHoursModel = new EmployeeHoursModel();

      string query = getEmployeeHoursContractQuery();

      try
      {
        var command = new SqlCommand(query, connectionRepository.connection);
        command.Parameters.AddWithValue("@loggedUserId", loggedUserId);

        var tableResult = connectionRepository.ExecuteQuery(command);

        if (tableResult.Rows.Count == 0)
        {
          throw new Exception("Ningún dato retornado del usuario.");
        }

        getEmployeeHoursContractFromTable(employeeHoursModel, tableResult);
      }
      catch (Exception ex)
      {
        throw new Exception("Error al obtener datos del contrato: "
          + ex.Message, ex);
      }

      return employeeHoursModel;
    }

    private string getEmployeeHoursListQuery()
    {
      return @"
        SELECT
          h.Fecha,
          h.horasTrabajadas,
          h.aprobadas,
          h.idSupervisor
        FROM Horas h
        WHERE idEmpleado = @loggedUserId 
          AND h.Fecha BETWEEN @startDate AND @endDate;
      ";
    }

    private void getEmployeeHoursListFromTable(List<HoursModel> hoursModel
      , DataTable resultTable)
    {
      foreach (DataRow row in resultTable.Rows)
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

    public List<HoursModel> getEmployeeHoursList(string loggedUserId
      , DateOnly startDate, DateOnly endDate)
    {

      List<HoursModel> hoursModel = new List<HoursModel>();

      string query = getEmployeeHoursListQuery();

      try
      {
        var command = new SqlCommand(query, connectionRepository.connection);

        command.Parameters.AddWithValue("@loggedUserId", loggedUserId);

        command.Parameters.AddWithValue("@startDate"
          , startDate.ToDateTime(TimeOnly.MinValue));

        command.Parameters.AddWithValue("@endDate"
          , endDate.ToDateTime(TimeOnly.MinValue));

        var resultTable = connectionRepository.ExecuteQuery(command);

        if (resultTable.Rows.Count == 0)
        {
          return hoursModel;
        }

        getEmployeeHoursListFromTable(hoursModel, resultTable);
      }
      catch (Exception ex)
      {
        throw new Exception("Error al obtener datos del contrato: "
          + ex.Message, ex);
      }
      return hoursModel;
    }

    private void registerEmployeeHoursQueryBuilder(string loggedUserId
      , List<HoursModel> employeeHoursWorked, SqlCommand command)
    {
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
    }

    public bool registerEmployeeHours(string loggedUserId
      , List<HoursModel> employeeHoursWorked)
    {
      bool success = false;

      var command = new SqlCommand();
      command.Connection = connectionRepository.connection;

      registerEmployeeHoursQueryBuilder(loggedUserId, employeeHoursWorked
        , command);

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
