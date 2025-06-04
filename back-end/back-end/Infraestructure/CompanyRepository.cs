using back_end.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace back_end.Infraestructure
{
  public class CompanyRepository
  {
    private readonly string _connectionRoute;

    public CompanyRepository()
    {
      var builder = WebApplication.CreateBuilder();
      _connectionRoute =
        builder.Configuration.GetConnectionString("InfinipayDBContext");
    }

    private SqlConnection GetConnection() =>
      new SqlConnection(_connectionRoute);
    private bool dataAlreadyExists(string table, string field, string value
      , SqlTransaction transaction)
    {
      var cmd = new SqlCommand(
          $"SELECT COUNT(*) FROM [{table}] WHERE [{field}] = @value",
          transaction.Connection, transaction);
      cmd.Parameters.AddWithValue("@value", value);
      var count = (int)cmd.ExecuteScalar();
      return count > 0;
    }
        private DataTable getQueryTable(string query)
    {
        using (var connection = GetConnection())
        using (var queryCommand = new SqlCommand(query, connection))
        using (var tableAdapter = new SqlDataAdapter(queryCommand))
        {
            var queryTable = new DataTable();
            connection.Open();
            tableAdapter.Fill(queryTable);
            return queryTable;
        }
    }

        public List<CompanyModel> GetAllCompanies()
        {
            var query = @"
        SELECT 
            p.identificacion,
            p.numeroTelefono,
            p.correoElectronico,
            pj.razonSocial,
            pj.descripcion,
            pj.tipoPago,
            pj.beneficiosPorEmpleado,
            d.provincia,
            d.canton,
            d.distrito,
            d.otrasSenas,
            a.fechaCreacion,
            u.nickname AS employerUsername
        FROM PersonaJuridica pj
        JOIN Persona p ON p.id = pj.id
        JOIN Direccion d ON d.idPersona = p.id
        JOIN Auditoria a ON a.id = p.idAuditoria
        JOIN Empleador e ON e.idPersonaJuridica = pj.id
        JOIN Usuario u ON u.idPersonaFisica = e.idPersonaFisica";

            var table = getQueryTable(query);
            var companies = new List<CompanyModel>();

            foreach (DataRow row in table.Rows)
            {
                var creationDate = Convert.ToDateTime(row["fechaCreacion"]);

                var company = new CompanyModel
                {
                    idNumber = row["identificacion"].ToString(),
                    phoneNumber = row["numeroTelefono"].ToString(),
                    email = row["correoElectronico"].ToString(),
                    legalName = row["razonSocial"].ToString(),
                    description = row["descripcion"].ToString(),
                    paymentType = row["tipoPago"].ToString(),
                    benefits = Convert.ToInt32(row["beneficiosPorEmpleado"]),
                    province = row["provincia"].ToString(),
                    canton = row["canton"].ToString(),
                    district = row["distrito"].ToString(),
                    otherSigns = row["otrasSenas"].ToString(),
                    creationDay = creationDate.Day,
                    creationMonth = creationDate.Month,
                    creationYear = creationDate.Year,
                    employerUsername = row["employerUsername"].ToString()
                };

                companies.Add(company);
            }

            return companies;
        }


        public Guid GetPersonIdByUsername(string username)
    {
      using (var connection = GetConnection())
      {
        connection.Open();
        var query =
          "SELECT idPersonaFisica FROM Usuario WHERE nickname = @username";
        using (var cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@username", username);
          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              return reader.GetGuid(0);
            }
          }
        }
      }
      throw new Exception("No se encontró el usuario con ese nombre.");
    }

    public bool createNewCompany(CompanyModel company)
    {
      using (var connection = GetConnection())
      {
        connection.Open();
        using (var transaction = connection.BeginTransaction())
        {
          try
          {
            if (dataAlreadyExists("Persona", "identificacion"
              , company.idNumber, transaction))
              throw new Exception("CEDULA_DUPLICADA");

            if (dataAlreadyExists("Persona", "numeroTelefono"
              , company.phoneNumber, transaction))
              throw new Exception("TELEFONO_DUPLICADO");

            if (dataAlreadyExists("Persona", "correoElectronico"
              , company.email, transaction))
              throw new Exception("EMAIL_DUPLICADO");

            Guid naturalPersonId =
              GetPersonIdByUsername(company.employerUsername);

            var auditId = insertAudit(company.employerUsername, transaction);
            var personId = insertPerson(company, auditId, transaction);

            insertLegalPerson(company, personId, transaction);
            insertAddress(company, personId, transaction);
            assignCompanyToEmployer(company, naturalPersonId, personId
              , transaction);

            transaction.Commit();
            Debug.WriteLine("Successful insertion in all tables.");
            return true;
          }
          catch (Exception ex)
          {
            Debug.WriteLine("Insert error: " + ex.Message);
            transaction.Rollback();
            throw;
          }
        }
      }
    }

    private Guid insertAudit(string username, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                INSERT INTO [dbo].[Auditoria] ([usuarioCreador])
                OUTPUT INSERTED.id
                VALUES (@usuarioCreador)", transaction.Connection
                , transaction);
      cmd.Parameters.AddWithValue("@usuarioCreador", username);
      var auditId = (Guid)cmd.ExecuteScalar();
      return auditId;
    }

    private Guid insertPerson(CompanyModel company, Guid auditId
      , SqlTransaction transaction)
    {
      string idType = "juridica";

      var cmd = new SqlCommand(@"
                INSERT INTO [dbo].[Persona]
                ([identificacion], [numeroTelefono], [correoElectronico],
                [tipoIdentificacion], [idAuditoria], [fechaNacimiento])
                OUTPUT INSERTED.id
                VALUES (@identificacion, @numeroTelefono, @correoElectronico,
                @tipoIdentificacion, @idAuditoria, @fechaNacimiento)",
          transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@identificacion", company.idNumber);
      cmd.Parameters.AddWithValue("@numeroTelefono", company.phoneNumber);
      cmd.Parameters.AddWithValue("@correoElectronico", company.email);
      cmd.Parameters.AddWithValue("@tipoIdentificacion", idType);
      cmd.Parameters.AddWithValue("@idAuditoria", auditId);
      var creationDate = new DateTime(company.creationYear
        , company.creationMonth, company.creationDay);
      cmd.Parameters.Add("@fechaNacimiento"
        , SqlDbType.Date).Value = creationDate;

      var personId = (Guid)cmd.ExecuteScalar();

      return personId;
    }

    private void insertLegalPerson(CompanyModel company, Guid personId
      , SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                INSERT INTO [dbo].[PersonaJuridica]
                ([id], [descripcion], [tipoPago], [beneficiosPorEmpleado],
                [razonSocial], [nombreAsociacion])
                VALUES (@id, @descripcion, @tipoPago, @beneficiosPorEmpleado,
                @razonSocial, @nombreAsociacion)",
          transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@id", personId);
      cmd.Parameters.AddWithValue("@descripcion"
        , company.description ?? (object)DBNull.Value);
      cmd.Parameters.AddWithValue("@tipoPago", company.paymentType);
      cmd.Parameters.AddWithValue("@beneficiosPorEmpleado", company.benefits);
      cmd.Parameters.AddWithValue("@razonSocial", company.legalName);
      cmd.Parameters.AddWithValue("@nombreAsociacion", company.associationName);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: insertLegalPerson.");
    }

    private void insertAddress(CompanyModel company, Guid personId
      , SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                INSERT INTO [dbo].[Direccion]
                ([idPersona], [provincia], [canton], [distrito], [otrasSenas])
                VALUES (@idPersona, @provincia, @canton, @distrito
                , @otrasSenas)",
          transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@idPersona", personId);
      cmd.Parameters.AddWithValue("@provincia", company.province);
      cmd.Parameters.AddWithValue("@canton", company.canton);
      cmd.Parameters.AddWithValue("@distrito", company.district);
      cmd.Parameters.AddWithValue("@otrasSenas"
        , company.otherSigns ?? (object)DBNull.Value);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: insertAddress.");
    }

    private void assignCompanyToEmployer(CompanyModel employer
      , Guid naturalPersonId, Guid personId, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                INSERT INTO [dbo].[Empleador]
                ([idPersonaFisica], [idPersonaJuridica])
                VALUES (@idPersonaFisica, @idPersonaJuridica)",
          transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@idPersonaFisica", naturalPersonId);
      cmd.Parameters.AddWithValue("@idPersonaJuridica", personId);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Insert failed: assignCompanyToEmployer.");
    }
  }
}
