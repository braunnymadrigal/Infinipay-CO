using back_end.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using back_end.Application;

namespace back_end.Repositories
{
    public class CompanyBenefitRepository : IBenefitRepository<CompanyBenefitDTO>
    {
        private SqlConnection _connection;
        private string _connectionRoute;
        private string _connectStringName = "InfinipayDBContext";

        public CompanyBenefitRepository()
        {
            var builder = WebApplication.CreateBuilder();

            _connectionRoute = builder.Configuration
                .GetConnectionString(this._connectStringName)
                ?? throw new Exception("Connection string not found.");

            _connection = new SqlConnection(_connectionRoute);
        }

        private void mapBenefit(CompanyBenefitDTO companyBenefit, DataRow column)
        {
            companyBenefit.benefit.id = column["id"] != DBNull.Value ? (Guid)column["id"] : Guid.Empty;
            companyBenefit.benefit.name = column["nombre"] != DBNull.Value ? Convert.ToString(column["nombre"]) : string.Empty;
            companyBenefit.benefit.minEmployeeTime = column["tiempoMinimo"] != DBNull.Value ? Convert.ToDecimal(column["tiempoMinimo"]) : 0;
            companyBenefit.benefit.description = column["descripcion"] != DBNull.Value ? Convert.ToString(column["descripcion"]) : string.Empty;
            companyBenefit.benefit.elegibleEmployees = column["empleadoElegible"] != DBNull.Value ? Convert.ToString(column["empleadoElegible"]) : string.Empty;
            companyBenefit.benefit.deductionType = column["tipoFormula"] != DBNull.Value ? Convert.ToString(column["tipoFormula"]) : string.Empty;
            companyBenefit.benefit.urlAPI = column["urlAPI"] != DBNull.Value ? Convert.ToString(column["urlAPI"]) : string.Empty;
            companyBenefit.benefit.paramOneAPI = column["paramUno"] != DBNull.Value ? Convert.ToString(column["paramUno"]) : string.Empty;
            companyBenefit.benefit.paramTwoAPI = column["paramDos"] != DBNull.Value ? Convert.ToString(column["paramDos"]) : string.Empty;
            companyBenefit.benefit.paramThreeAPI = column["paramTres"] != DBNull.Value ? Convert.ToString(column["paramTres"]) : string.Empty;
            companyBenefit.benefit.userCreator = column["usuarioCreador"] != DBNull.Value ? Convert.ToString(column["usuarioCreador"]) : string.Empty;
            companyBenefit.benefit.creationDate = column["fechaCreacion"] != DBNull.Value ? Convert.ToDateTime(column["fechaCreacion"]) : DateTime.MinValue;
            companyBenefit.benefit.userModifier = column["ultimoUsuarioModificador"] != DBNull.Value ? Convert.ToString(column["ultimoUsuarioModificador"]) : string.Empty;
            companyBenefit.benefit.modifiedDate = column["ultimaFechaModificacion"] != DBNull.Value ? Convert.ToDateTime(column["ultimaFechaModificacion"]) : DateTime.MinValue;
        }

        private DataTable GetQueryTable(string query, SqlParameter[] parameters)
        {
            DataTable queryTable = new DataTable();

            try
            {
                using (SqlCommand queryCommand = new SqlCommand(query, _connection))
                using (SqlDataAdapter tableAdapter = new SqlDataAdapter(queryCommand))
                {
                    if (parameters != null && parameters.Length > 0)
                        queryCommand.Parameters.AddRange(parameters);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    tableAdapter.Fill(queryTable);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar consulta con parámetros: " + ex.Message, ex);
            }
            finally
            {
                _connection.Close();
            }

            return queryTable;
        }

        private bool dataAlreadyExists(string table, string field, string value, Guid? idPersonaJuridica, SqlTransaction transaction)
        {
            var cmd = new SqlCommand(
                $"SELECT COUNT(*) FROM [{table}] WHERE [{field}] = @value AND idPersonaJuridica = @empresaId",
                transaction.Connection, transaction);

            cmd.Parameters.AddWithValue("@value", value);
            cmd.Parameters.AddWithValue("@empresaId", idPersonaJuridica);

            var count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        private Guid getCompanyId(string nickname, SqlTransaction transaction)
        {
            var cmd = new SqlCommand(
                "SELECT pj.id FROM PersonaJuridica pj " +
                "JOIN Empleador em ON em.idPersonaJuridica = pj.id " +
                "JOIN Usuario u ON em.idPersonaFisica = u.idPersonaFisica " +
                "WHERE u.nickname = @nickname", transaction.Connection, transaction);

            cmd.Parameters.AddWithValue("@nickname", nickname);
            var result = cmd.ExecuteScalar();
            return result != null ? (Guid)result : Guid.Empty;
        }

        private bool hasEmployeeWithBenefit(Guid benefitId, SqlTransaction transaction)
        {
            var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM BeneficioPorEmpleado WHERE idBeneficio = @benefitId",
                transaction.Connection, transaction);

            cmd.Parameters.AddWithValue("@benefitId", benefitId);
            var count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        public List<CompanyBenefitDTO> getBenefits(string nickname)
        {
            var query = @"
                SELECT
                    b.id, b.nombre, b.tiempoMinimo, b.descripcion, b.empleadoElegible,
                    f.tipoFormula, f.urlAPI, f.paramUno, f.paramDos, f.paramTres,
                    a.usuarioCreador, a.fechaCreacion, a.ultimoUsuarioModificador, a.ultimaFechaModificacion
                FROM Beneficio b
                    JOIN PersonaJuridica pj ON pj.id = b.idPersonaJuridica
                    JOIN Empleador em ON em.idPersonaJuridica = b.idPersonaJuridica
                    JOIN Usuario u ON em.idPersonaFisica = u.idPersonaFisica
                    JOIN Auditoria a ON a.id = b.idAuditoria
                    JOIN Deduccion d ON d.idBeneficio = b.id
                    JOIN Formula f ON f.id = d.idFormula
                WHERE u.nickname = @nickname;
            ";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@nickname", SqlDbType.VarChar) { Value = nickname }
            };

            var table = GetQueryTable(query, parameters);
            var benefits = new List<CompanyBenefitDTO>();

            foreach (DataRow row in table.Rows)
            {
                var companyBenefit = new CompanyBenefitDTO
                {
                    benefit = new BenefitDTO()
                };
                mapBenefit(companyBenefit, row);
                benefits.Add(companyBenefit);
            }

            return benefits;
        }

        public CompanyBenefitDTO getBenefitById(Guid id)
        {
            var query = @"
                SELECT
                    b.id, b.nombre, b.tiempoMinimo, b.descripcion, b.empleadoElegible,
                    f.tipoFormula, f.urlAPI, f.paramUno, f.paramDos, f.paramTres,
                    a.usuarioCreador, a.fechaCreacion, a.ultimoUsuarioModificador, a.ultimaFechaModificacion
                FROM Beneficio b
                    JOIN PersonaJuridica pj ON pj.id = b.idPersonaJuridica
                    JOIN Empleador em ON em.idPersonaJuridica = b.idPersonaJuridica
                    JOIN Usuario u ON em.idPersonaFisica = u.idPersonaFisica
                    JOIN Auditoria a ON a.id = b.idAuditoria
                    JOIN Deduccion d ON d.idBeneficio = b.id
                    JOIN Formula f ON f.id = d.idFormula
                WHERE b.id = @id;
            ";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.UniqueIdentifier) { Value = id }
            };

            var table = GetQueryTable(query, parameters);

            if (table.Rows.Count == 0)
                return null;

            var companyBenefit = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO()
            };
            mapBenefit(companyBenefit, table.Rows[0]);
            return companyBenefit;
        }

        private CompanyBenefitDTO getBenefitById(Guid id, SqlTransaction transaction)
        {
            var query = @"
                SELECT
                    b.id,
                    b.nombre,
                    b.tiempoMinimo,
                    b.descripcion,
                    b.empleadoElegible,
                    f.tipoFormula,
                    f.urlAPI,
                    f.paramUno,
                    f.paramDos,
                    f.paramTres,
                    a.usuarioCreador,
                    a.fechaCreacion,
                    a.ultimoUsuarioModificador,
                    a.ultimaFechaModificacion
                FROM Beneficio b
                    JOIN PersonaJuridica pj ON pj.id = b.idPersonaJuridica
                    JOIN Empleador em ON em.idPersonaJuridica = b.idPersonaJuridica
                    JOIN Usuario u ON em.idPersonaFisica = u.idPersonaFisica
                    JOIN Auditoria a ON a.id = b.idAuditoria
                    JOIN Deduccion d ON d.idBeneficio = b.id
                    JOIN Formula f ON f.id = d.idFormula
                WHERE b.id = @id;
            ";

            var cmd = new SqlCommand(query, transaction.Connection, transaction);
            cmd.Parameters.AddWithValue("@id", id);

            var benefit = new CompanyBenefitDTO { benefit = new BenefitDTO() };

            using (var adapter = new SqlDataAdapter(cmd))
            {
                var table = new DataTable();
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    mapBenefit(benefit, table.Rows[0]);
                    return benefit;
                }
            }

            return null;
        }


        public bool CreateBenefit(CompanyBenefitDTO companyBenefit, string loggedUserNickname)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (var transaction = _connection.BeginTransaction())
                {
                    companyBenefit.benefit.companyId = getCompanyId(loggedUserNickname, transaction);
                    if (!companyBenefit.benefit.companyId.HasValue)
                        throw new Exception("EMPRESA_NO_ENCONTRADA");

                    if (dataAlreadyExists("Beneficio", "nombre", companyBenefit.benefit.name, companyBenefit.benefit.companyId, transaction))
                        throw new Exception("BENEFICIO_DUPLICADO");
                    using (var cmd = new SqlCommand("StoreBenefit", _connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User", loggedUserNickname);
                        cmd.Parameters.AddWithValue("@FormulaType", companyBenefit.benefit.deductionType);
                        cmd.Parameters.AddWithValue("@UrlApi", (object?)companyBenefit.benefit.urlAPI ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Param1", companyBenefit.benefit.paramOneAPI);
                        cmd.Parameters.AddWithValue("@Param2", (object?)companyBenefit.benefit.paramTwoAPI ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Param3", (object?)companyBenefit.benefit.paramThreeAPI ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Name", companyBenefit.benefit.name);
                        cmd.Parameters.AddWithValue("@CompanyId", companyBenefit.benefit.companyId);
                        cmd.Parameters.AddWithValue("@MinTime", companyBenefit.benefit.minEmployeeTime);
                        cmd.Parameters.AddWithValue("@Description", companyBenefit.benefit.description);
                        cmd.Parameters.AddWithValue("@ElegibleEmployees", (object?)companyBenefit.benefit.elegibleEmployees ?? "todos");

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en CreateBenefit: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public void UpdateBenefit(Guid id, CompanyBenefitDTO companyBenefit, string loggedUserNickname)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (var transaction = _connection.BeginTransaction())
                {
                    companyBenefit.benefit.companyId = getCompanyId(loggedUserNickname, transaction);
                    if (!companyBenefit.benefit.companyId.HasValue)
                        throw new Exception("EMPRESA_NO_ENCONTRADA");

                    var existingBenefit = getBenefitById(id, transaction);
                    if (existingBenefit == null)
                        throw new Exception("BENEFICIO_NO_ENCONTRADO");

                    if (existingBenefit.benefit.name != companyBenefit.benefit.name &&
                        dataAlreadyExists("Beneficio", "nombre", companyBenefit.benefit.name, companyBenefit.benefit.companyId, transaction))
                        throw new Exception("BENEFICIO_DUPLICADO");

                    if (existingBenefit.benefit.urlAPI != companyBenefit.benefit.urlAPI &&
                        (existingBenefit.benefit.paramOneAPI != companyBenefit.benefit.paramOneAPI ||
                         existingBenefit.benefit.paramTwoAPI != companyBenefit.benefit.paramTwoAPI ||
                         existingBenefit.benefit.paramThreeAPI != companyBenefit.benefit.paramThreeAPI))
                    {
                        if (hasEmployeeWithBenefit(id, transaction))
                            throw new Exception("BENEFICIO_NO_PUEDE_SER_MODIFICADO");
                    }

                    using (var cmd = new SqlCommand("UpdateBenefit", _connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@User", loggedUserNickname);
                        cmd.Parameters.AddWithValue("@FormulaType", companyBenefit.benefit.deductionType);
                        cmd.Parameters.AddWithValue("@UrlApi", (object?)companyBenefit.benefit.urlAPI ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Param1", companyBenefit.benefit.paramOneAPI);
                        cmd.Parameters.AddWithValue("@Param2", (object?)companyBenefit.benefit.paramTwoAPI ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Param3", (object?)companyBenefit.benefit.paramThreeAPI ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Name", companyBenefit.benefit.name);
                        cmd.Parameters.AddWithValue("@CompanyId", companyBenefit.benefit.companyId);
                        cmd.Parameters.AddWithValue("@MinTime", companyBenefit.benefit.minEmployeeTime);
                        cmd.Parameters.AddWithValue("@Description", companyBenefit.benefit.description);
                        cmd.Parameters.AddWithValue("@ElegibleEmployees", (object?)companyBenefit.benefit.elegibleEmployees ?? "todos");

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el beneficio: " + ex.Message, ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
    }
}
