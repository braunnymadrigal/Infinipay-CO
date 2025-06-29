using System.Data;
using System.Diagnostics;
using System.Text;
using Microsoft.Data.SqlClient;

namespace back_end.Infraestructure
{
  public class EmailQueryRepository : IEmailQueryRepository
  {
    private readonly AbstractConnectionRepository connectionRepository;

    public EmailQueryRepository(AbstractConnectionRepository connectionRepository)
    {
      this.connectionRepository = connectionRepository;
    }

    private string getEmailQuery()
    {
      return @"SELECT p.correoElectronico FROM Persona p
       WHERE p.id = @loggedUserId";
    }

    private string getEmailFromTable(DataTable table)
    {
      if (table.Rows.Count == 0) return string.Empty;

      return table.Rows[0]["correoElectronico"] is DBNull
          ? string.Empty
          : Convert.ToString(table.Rows[0]["correoElectronico"]);
    }

    public string getEmail(string loggedUserId)
    {
      var query = getEmailQuery();

      try
      {
        var command = new SqlCommand(query, connectionRepository.connection);
        
        command.Parameters.AddWithValue("@loggedUserId", loggedUserId);
        var resultTable = connectionRepository.ExecuteQuery(command);
        return getEmailFromTable(resultTable);
      }
      catch (Exception ex)
      {
        throw new Exception("Error obteniendo correo electrónico del usuario");
      }
    }
  }
}
