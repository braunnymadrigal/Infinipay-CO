using back_end.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace back_end.Infraestructure
{
  public class EmployeeRepository
  {
    private readonly string _connectionRoute;

    public EmployeeRepository()
    {
      var builder = WebApplication.CreateBuilder();
      _connectionRoute =
        builder.Configuration.GetConnectionString("InfinipayDBContext");
    }

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

    private bool IsDataValid(EmployeeModel employee, SqlTransaction transaction)
    {
      if (dataAlreadyExists("Persona", "identificacion"
        , employee.idNumber, transaction)) {
        throw new Exception("CEDULA_DUPLICADA");
        }

      if (dataAlreadyExists("Persona", "numeroTelefono"
        , employee.phoneNumber, transaction)) {
        throw new Exception("TELEFONO_DUPLICADO");
        }

      if (dataAlreadyExists("Persona", "correoElectronico"
        , employee.email, transaction)) {
        throw new Exception("EMAIL_DUPLICADO");
        }

      if (dataAlreadyExists("Usuario", "nickname"
        , employee.username, transaction)) {
        throw new Exception("USERNAME_DUPLICADO");
        }

      return true;
    }

    private SqlConnection GetConnection() =>
      new SqlConnection(_connectionRoute);

    public string getUsernameByPersonId(string personId)
    {
      using (var connection = GetConnection())
      {
        connection.Open();
        var query =
          "SELECT nickname FROM Usuario WHERE idPersonaFisica = @personId";
        using (var cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@personId", personId);
          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              return reader.GetString(0);
            }
          }
        }
      }
      throw new Exception("No se encontró un usuario con ese ID.");
    }

    public EmployeeModel GetEmployeeById(Guid id)
    {
      using (var connection = GetConnection())
      {
        connection.Open();
        var query = @"
              SELECT p.correoElectronico, p.identificacion, 
             p.numeroTelefono, p.fechaNacimiento,
             pf.primerNombre, pf.segundoNombre, 
             pf.primerApellido, pf.segundoApellido, 
             pf.genero, d.provincia, d.canton, d.distrito, 
             d.otrasSenas, e.rol, e.fechaContratacion, u.nickname,
             c.fechaCreacion, c.reportaHoras, c.salarioBruto, 
             c.tipoContrato 
            FROM Empleado e 
            JOIN Persona p ON p.id = e.idPersonaFisica
            JOIN PersonaFisica pf ON pf.id = e.idPersonaFisica 
            JOIN Direccion d ON d.idPersona = p.id
            JOIN Usuario u ON u.idPersonaFisica = pf.id 
            JOIN Contrato c ON c.idEmpleado = e.idPersonaFisica
            WHERE p.id = @id";

        using (var cmd = new SqlCommand(query, connection))
        {
          cmd.Parameters.AddWithValue("@id", id);

          using (var reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              try
              {
                return mapEmployee(reader);
              }
              catch (Exception ex)
              {
                Debug.WriteLine("Error reading employee data: " + ex.Message);
                throw new Exception("Error al leer los datos del empleado.");
              }
            }
          }
        }
      }
      throw new Exception("Empleado no encontrado.");
    }

    private EmployeeModel mapEmployee(SqlDataReader reader)
    {
      var fechaNacimiento = reader.GetDateTime(reader.GetOrdinal("fechaNacimiento"));
      var fechaContratacion = reader.GetDateTime(reader.GetOrdinal("fechaContratacion"));
      var fechaCreacion = reader.GetDateTime(reader.GetOrdinal("fechaCreacion"));

      return new EmployeeModel
      {
          email = reader["correoElectronico"].ToString(),
          idNumber = reader["identificacion"].ToString(),
          phoneNumber = reader["numeroTelefono"].ToString(),
          firstName = reader["primerNombre"].ToString(),
          secondName = reader["segundoNombre"] as string,
          firstLastName = reader["primerApellido"].ToString(),
          secondLastName = reader["segundoApellido"].ToString(),
          gender = reader["genero"].ToString(),
          province = reader["provincia"].ToString(),
          canton = reader["canton"].ToString(),
          district = reader["distrito"].ToString(),
          otherSigns = reader["otrasSenas"].ToString(),
          role = reader["rol"].ToString(),
          username = reader["nickname"].ToString(),
          birthDay = fechaNacimiento.Day,
          birthMonth = fechaNacimiento.Month,
          birthYear = fechaNacimiento.Year,
          hireDay = fechaContratacion.Day,
          hireMonth = fechaContratacion.Month,
          hireYear = fechaContratacion.Year,
          creationDay = fechaCreacion.Day,
          creationMonth = fechaCreacion.Month,
          creationYear = fechaCreacion.Year,
          salary = Convert.ToInt32(reader["salarioBruto"]),
          reportsHours = Convert.ToInt32(reader["reportaHoras"]),
          typeContract = reader["tipoContrato"].ToString()
      };
    }

    public bool createNewEmployee(EmployeeModel employee, string loggedId)
    {
      using (var connection = GetConnection())
      {
        connection.Open();
        using (var transaction = connection.BeginTransaction())
        {
          try
          {
            var cmd = new SqlCommand("sp_CreateNewEmployee", connection
              , transaction)
            {
              CommandType = CommandType.StoredProcedure
            };

            addAuditParameters(cmd, loggedId);
            addPersonParameters(cmd, employee);
            addNaturalPersonParameters(cmd, employee);
            addAddressParameters(cmd, employee);
            addUserParameters(cmd, employee);
            addEmployeeParameters(cmd, employee, loggedId);
            addContractParameters(cmd, employee);

            cmd.ExecuteNonQuery();
            transaction.Commit();
            return true;
          }
          catch (SqlException ex)
          {
            Debug.WriteLine("Stored procedure error: " + ex.Message);
            transaction.Rollback();
            throw;
          }
        }
      }
    }

    public void UpdateEmployeeData(EmployeeModel employee, Guid id, string loggedUsername)
    {
      using (var connection = GetConnection())
      {
      
        connection.Open();
        using (var transaction = connection.BeginTransaction())
        {
          try
          {
            var currentData = GetEmployeeCurrentData(id, transaction);
            if (currentData == null)
            {
              throw new Exception("EMPLOYEE_NOT_FOUND");
            }

            if (currentData.idNumber != employee.idNumber
              || currentData.firstName != employee.firstName ||
              currentData.secondName != employee.secondName ||
              currentData.firstLastName != employee.firstLastName ||
              currentData.secondLastName != employee.secondLastName)
            {
              if (hasEmployeeBeenPayedAlready(id, transaction))
              {
                throw new Exception("EMPLOYEE_ALREADY_PAYED");
              }
            }

            if (employee.idNumber != currentData.idNumber) {
              if (dataAlreadyExists("Persona", "identificacion"
                , employee.idNumber, transaction)) {
                throw new Exception("CEDULA_DUPLICADA");
              }
            }
            if (employee.phoneNumber != currentData.phoneNumber) {
              if (dataAlreadyExists("Persona", "numeroTelefono"
                , employee.phoneNumber, transaction)) {
                throw new Exception("TELEFONO_DUPLICADO");
              }
            }
            if (employee.email != currentData.email) {
              if (dataAlreadyExists("Persona", "correoElectronico"
                , employee.email, transaction)) {
                throw new Exception("EMAIL_DUPLICADO");
              }
            }
            if (employee.username != currentData.username) {
              if (dataAlreadyExists("Usuario", "nickname"
                , employee.username, transaction)) {
                throw new Exception("USERNAME_DUPLICADO");
              }
            }
            
            UpdatePerson(employee, id, transaction);
            
            UpdateNaturalPerson(employee, id, transaction);
            
            UpdateAddress(employee, id, transaction);
            
            UpdateUser(employee, id, transaction);
            
            UpdateEmployeeDetails(employee, id, transaction);
            
            UpdateEmployeeContractDetails(employee, id, transaction);

            UpdateEmployeeAudit(id, loggedUsername, transaction);
            
            Debug.WriteLine("Employee data updated successfully.");
            transaction.Commit();
          }
          catch (Exception ex)
          {
            transaction.Rollback();
            throw new Exception("Error updating employee data: " + ex.Message);
          }

        }
      }  
    }

    private void addAuditParameters(SqlCommand cmd, string loggedId)
    {
      string loggedUsername = getUsernameByPersonId(loggedId);
      cmd.Parameters.AddWithValue("@loggedPersonId", Guid.Parse(loggedId));

    }

    private void addPersonParameters(SqlCommand cmd, EmployeeModel employee)
    {
      cmd.Parameters.AddWithValue("@birthDay", employee.birthDay);
      cmd.Parameters.AddWithValue("@birthMonth", employee.birthMonth);
      cmd.Parameters.AddWithValue("@birthYear", employee.birthYear);
      cmd.Parameters.AddWithValue("@idNumber", employee.idNumber);
      cmd.Parameters.AddWithValue("@phoneNumber", employee.phoneNumber);
      cmd.Parameters.AddWithValue("@email", employee.email);

    }

    private void addNaturalPersonParameters(SqlCommand cmd
      , EmployeeModel employee)
    {
      cmd.Parameters.AddWithValue("@firstName", employee.firstName);
      cmd.Parameters.AddWithValue("@secondName"
        , (object?)employee.secondName ?? DBNull.Value);
      cmd.Parameters.AddWithValue("@firstLastName", employee.firstLastName);
      cmd.Parameters.AddWithValue("@secondLastName", employee.secondLastName);
      cmd.Parameters.AddWithValue("@gender", employee.gender);
    }

    private void addAddressParameters(SqlCommand cmd, EmployeeModel employee)
    {
      cmd.Parameters.AddWithValue("@province", employee.province);
      cmd.Parameters.AddWithValue("@canton", employee.canton);
      cmd.Parameters.AddWithValue("@district", employee.district);
      cmd.Parameters.AddWithValue("@otherSigns"
        , (object?)employee.otherSigns ?? DBNull.Value);
    }

    private void addUserParameters(SqlCommand cmd, EmployeeModel employee)
    {
      var birthDate = new DateTime(employee.birthYear, employee.birthMonth
        , employee.birthDay);
      var rawPassword = employee.firstLastName +
        birthDate.ToString("ddMMyyyy") + "!";
      cmd.Parameters.AddWithValue("@username", employee.username);
      cmd.Parameters.AddWithValue("@password", rawPassword);
    }

    private void addEmployeeParameters(SqlCommand cmd, EmployeeModel employee
      , string loggedId)
    {
      cmd.Parameters.AddWithValue("@hireDay", employee.hireDay);
      cmd.Parameters.AddWithValue("@hireMonth", employee.hireMonth);
      cmd.Parameters.AddWithValue("@hireYear", employee.hireYear);
      cmd.Parameters.AddWithValue("@role"
        , (object?)employee.role ?? DBNull.Value);

    }

    private void addContractParameters(SqlCommand cmd, EmployeeModel employee)
    {
      cmd.Parameters.AddWithValue("@creationDay", employee.creationDay);
      cmd.Parameters.AddWithValue("@creationMonth", employee.creationMonth);
      cmd.Parameters.AddWithValue("@creationYear", employee.creationYear);
      cmd.Parameters.AddWithValue("@reportsHours", employee.reportsHours);
      cmd.Parameters.AddWithValue("@salary", employee.salary);
      cmd.Parameters.AddWithValue("@typeContract", employee.typeContract);

    } 
    
    private EmployeeModel GetEmployeeCurrentData(Guid id, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
          SELECT p.identificacion, pf.primerNombre, pf.segundoNombre, pf.primerApellido, pf.segundoApellido,
          p.numeroTelefono, correoElectronico, u.nickname
	        FROM Persona p JOIN PersonaFisica pf on p.id = pf.id JOIN Usuario u on p.id = u.idPersonaFisica
	        WHERE p.id = @id", transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@id", id);

      using (var reader = cmd.ExecuteReader())
      {
        if (reader.Read())
        {
          var employee = new EmployeeModel
          {
            idNumber = reader.GetString(0),
            firstName = reader.GetString(1),
            secondName = reader.IsDBNull(2) ? null : reader.GetString(2),
            firstLastName = reader.GetString(3),
            secondLastName = reader.GetString(4),
            phoneNumber = reader.GetString(5),
            email = reader.GetString(6),
            username = reader.GetString(7)
          };
          return employee;
        }
      }
      return null;
    }

    private bool hasEmployeeBeenPayedAlready(Guid id, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
        SELECT TOP 1 COUNT(*) FROM [dbo].[DetallePago]
        WHERE [idEmpleado] = @id", transaction.Connection, transaction);
      cmd.Parameters.AddWithValue("@id", id);
      var count = (int)cmd.ExecuteScalar();
      return count > 0;
    }

    private void UpdatePerson(EmployeeModel employee, Guid id
      , SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                UPDATE Persona SET identificacion = @identificacion, numeroTelefono = @numeroTelefono, 
                correoElectronico = @correoElectronico, fechaNacimiento = @fechaNacimiento 
                WHERE id = @id", transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@identificacion", employee.idNumber);
      cmd.Parameters.AddWithValue("@numeroTelefono", employee.phoneNumber);
      cmd.Parameters.AddWithValue("@correoElectronico", employee.email);
      var birthDate = new DateTime(employee.birthYear
        , employee.birthMonth, employee.birthDay);
      cmd.Parameters.AddWithValue("@fechaNacimiento", birthDate);
      cmd.Parameters.AddWithValue("@id", id);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Update failed: UpdatePerson.");
    }

    private void UpdateNaturalPerson(EmployeeModel employee, Guid id, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                UPDATE [dbo].[PersonaFisica]
                SET [primerNombre] = @primerNombre,
                    [segundoNombre] = @segundoNombre,
                    [primerApellido] = @primerApellido,
                    [segundoApellido] = @segundoApellido,
                    [genero] = @genero
                WHERE [id] = @id", transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@primerNombre", employee.firstName);
      cmd.Parameters.AddWithValue("@segundoNombre", employee.secondName);
      cmd.Parameters.AddWithValue("@primerApellido", employee.firstLastName);
      cmd.Parameters.AddWithValue("@segundoApellido", employee.secondLastName);
      cmd.Parameters.AddWithValue("@genero", employee.gender);
      cmd.Parameters.AddWithValue("@id", id);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Update failed: UpdateNaturalPerson.");
    }

    private void UpdateAddress(EmployeeModel employee, Guid id, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                UPDATE [dbo].[Direccion]
                SET [provincia] = @provincia,
                    [canton] = @canton,
                    [distrito] = @distrito,
                    [otrasSenas] = @otrasSenas
                WHERE [idPersona] = @id", transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@provincia", employee.province);
      cmd.Parameters.AddWithValue("@canton", employee.canton);
      cmd.Parameters.AddWithValue("@distrito", employee.district);
      cmd.Parameters.AddWithValue("@otrasSenas", employee.otherSigns);
      cmd.Parameters.AddWithValue("@id", id);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Update failed: UpdateAddress.");
    }

    private void UpdateUser(EmployeeModel employee, Guid id, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                UPDATE [dbo].[Usuario]
                SET [nickname] = @nickname
                WHERE [idPersonaFisica] = @id", transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@nickname", employee.username);
      cmd.Parameters.AddWithValue("@id", id);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Update failed: UpdateUser.");
    }

    private void UpdateEmployeeDetails(EmployeeModel employee, Guid id
      , SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                UPDATE [dbo].[Empleado]
                SET [rol] = @rol,
                    [fechaContratacion] = @fechaContratacion
                WHERE [idPersonaFisica] = @id", transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@rol", employee.role);
      var hireDate = new DateTime(employee.hireYear, employee.hireMonth
        , employee.hireDay);
      cmd.Parameters.AddWithValue("@fechaContratacion", hireDate);
      cmd.Parameters.AddWithValue("@id", id);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Update failed: UpdateEmployeeDetails.");
    }

    private void UpdateEmployeeContractDetails(EmployeeModel employee, Guid id, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
        UPDATE Contrato SET reportaHoras = @reportaHoras, fechaCreacion = @fechaCreacion, salarioBruto = @salarioBruto,
		    tipoContrato = @tipoContrato WHERE idEmpleado = @id", transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@reportaHoras", employee.reportsHours);
      cmd.Parameters.AddWithValue("@salarioBruto", employee.salary);
      var creationDate = new DateTime(employee.creationYear
        , employee.creationMonth, employee.creationDay);
      cmd.Parameters.Add("@fechaCreacion", SqlDbType.Date).Value = creationDate;
      cmd.Parameters.AddWithValue("@tipoContrato", employee.typeContract);
      cmd.Parameters.AddWithValue("@id", id);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Update failed: UpdateEmployeeContractDetails.");
    }

    private void UpdateEmployeeAudit(Guid EmployeeId, string loggedUsername, SqlTransaction transaction)
    {
        var cmd = new SqlCommand(@"
        UPDATE a SET a.ultimoUsuarioModificador = @ultimoUsuarioModificador
        FROM Auditoria a JOIN Persona p ON p.IdAuditoria = a.id
          WHERE p.id = @id;", transaction.Connection, transaction);

          cmd.Parameters.AddWithValue("@ultimoUsuarioModificador", loggedUsername);
          cmd.Parameters.AddWithValue("@id", EmployeeId);

        if (cmd.ExecuteNonQuery() < 1)
          throw new Exception("Update failed: UpdateEmployeeAudit.");
        
    }
  }
}

