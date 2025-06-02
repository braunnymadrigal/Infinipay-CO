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

    private bool IsDataValid(EmployeeModel employee, SqlTransaction transaction) {
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

    public void UpdateEmployeeData(EmployeeModel employee, Guid id)
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

            if (!UpdatePerson(employee, id, transaction))
            {
              throw new Exception("Error updating person data.");
            }

            if (!UpdateNaturalPerson(employee, id, transaction))
            {
              throw new Exception("Error updating natural person data.");
            }
            if (!UpdateAddress(employee, id, transaction))
            {
              throw new Exception("Error updating address data.");
            }
            if (!UpdateUser(employee, id, transaction))
            {
              throw new Exception("Error updating user data.");
            }
            if (!UpdateEmployeeDetails(employee, id, transaction))
            {
              throw new Exception("Error updating employee details.");
            }
            if (!UpdateEmployeeContractDetails(employee, id, transaction))
            {
              throw new Exception("Error updating employee contract details.");
            }
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

    private Guid insertAudit(string username, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                INSERT INTO [dbo].[Auditoria] ([usuarioCreador])
                OUTPUT INSERTED.id
                VALUES (@usuarioCreador)", transaction.Connection
                , transaction);
      cmd.Parameters.AddWithValue("@usuarioCreador", username);
      var auditId = (Guid)cmd.ExecuteScalar();
      Debug.WriteLine("Inserted audit with ID: " + auditId);
      return auditId;
    }

    private bool UpdatePerson(EmployeeModel employee, Guid id
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
      return true;
    }

    private bool UpdateNaturalPerson(EmployeeModel employee, Guid id, SqlTransaction transaction)
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
      return true;
    }

    private bool UpdateAddress(EmployeeModel employee, Guid id, SqlTransaction transaction)
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
      return true;
    }

    private bool UpdateUser(EmployeeModel employee, Guid id, SqlTransaction transaction)
    {
      var cmd = new SqlCommand(@"
                UPDATE [dbo].[Usuario]
                SET [nickname] = @nickname,
                    [contrasena] = HASHBYTES('SHA2_512', CONVERT(varchar(100), @contrasena))
                WHERE [idPersonaFisica] = @id", transaction.Connection, transaction);

      cmd.Parameters.AddWithValue("@nickname", employee.username);
      cmd.Parameters.AddWithValue("@contrasena", employee.password);
      cmd.Parameters.AddWithValue("@id", id);

      if (cmd.ExecuteNonQuery() < 1)
        throw new Exception("Update failed: UpdateUser.");
      return true;
    }

    private bool UpdateEmployeeDetails(EmployeeModel employee, Guid id
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
      return true;

    }

    private bool UpdateEmployeeContractDetails(EmployeeModel employee, Guid id, SqlTransaction transaction)
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
      return true;
    }
  }
}

