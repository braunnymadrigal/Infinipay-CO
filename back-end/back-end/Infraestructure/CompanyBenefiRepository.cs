using back_end.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;

namespace back_end.Repositories
{
    public class CompanyBenefitRepository
    {
        private readonly string _connectionRoute;
        private SqlConnection _connection;

        public CompanyBenefitRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionRoute = builder.Configuration.GetConnectionString("InfinipayDBContext");
            _connection = new SqlConnection(_connectionRoute);
        }

        private void mapBenefit(CompanyBenefitDTO companyBenefit, DataRow column)
        {
            companyBenefit.benefit.id = column["id"] != DBNull.Value
                ? (Guid)column["id"] : Guid.Empty;

            companyBenefit.benefit.name = column["nombre"] != DBNull.Value
                ? Convert.ToString(column["nombre"]) : string.Empty;

            companyBenefit.benefit.minEmployeeTime = column["tiempoMinimo"] != DBNull.Value
                ? Convert.ToDecimal(column["tiempoMinimo"]) : 0;

            companyBenefit.benefit.description = column["descripcion"] != DBNull.Value
                ? Convert.ToString(column["descripcion"]) : string.Empty;

            companyBenefit.benefit.elegibleEmployees = column["empleadoElegible"] != DBNull.Value
                ? Convert.ToString(column["empleadoElegible"]) : string.Empty;

            companyBenefit.benefit.deductionType = column["tipoFormula"] != DBNull.Value
                ? Convert.ToString(column["tipoFormula"]) : string.Empty;

            companyBenefit.benefit.urlAPI = column["urlAPI"] != DBNull.Value
                ? Convert.ToString(column["urlAPI"]) : string.Empty;

            companyBenefit.benefit.paramOneAPI = column["paramUno"] != DBNull.Value
                ? Convert.ToString(column["paramUno"]) : string.Empty;

            companyBenefit.benefit.paramTwoAPI = column["paramDos"] != DBNull.Value
                ? Convert.ToString(column["paramDos"]) : string.Empty;

            companyBenefit.benefit.paramThreeAPI = column["paramTres"] != DBNull.Value
                ? Convert.ToString(column["paramTres"]) : string.Empty;

            companyBenefit.benefit.userCreator = column["usuarioCreador"] != DBNull.Value
                ? Convert.ToString(column["usuarioCreador"]) : string.Empty;

            companyBenefit.benefit.creationDate = column["fechaCreacion"] != DBNull.Value
                ? Convert.ToDateTime(column["fechaCreacion"]) : DateTime.MinValue;

            companyBenefit.benefit.userModifier = column["ultimoUsuarioModificador"] != DBNull.Value
                ? Convert.ToString(column["ultimoUsuarioModificador"]) : string.Empty;

            companyBenefit.benefit.modifiedDate = column["ultimaFechaModificacion"] != DBNull.Value
                ? Convert.ToDateTime(column["ultimaFechaModificacion"]) : DateTime.MinValue;
        }


        private DataTable GetQueryTable(string query, SqlParameter[] parameters)
        {
            DataTable queryTable = new DataTable();

            using (var connection = new SqlConnection(_connectionRoute))
            {
                using (SqlCommand queryCommand = new SqlCommand(query, connection))
                using (SqlDataAdapter tableAdapter = new SqlDataAdapter(queryCommand))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        queryCommand.Parameters.AddRange(parameters);
                    }

                    if (_connection.State == ConnectionState.Open)
                    {
                        _connection.Open();
                    }

                    tableAdapter.Fill(queryTable);
                }
            }

            return queryTable;
        }

        public List<CompanyBenefitDTO> getBenefits(string nickname)
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
                var companyBenefit = new CompanyBenefitDTO{
                    benefit = new BenefitDTO()
                };
                mapBenefit(companyBenefit, row);
                benefits.Add(companyBenefit);
            }

            return benefits;
        }

        public bool CreateBenefit(CompanyBenefitDTO companyBenefit, string loggedUserNickname)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (var transaction = _connection.BeginTransaction())
                {
                    try
                    {
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
                            cmd.Parameters.AddWithValue("@MinTime", companyBenefit.benefit.minEmployeeTime);
                            cmd.Parameters.AddWithValue("@Description", companyBenefit.benefit.description);
                            cmd.Parameters.AddWithValue("@ElegibleEmployees", (object?)companyBenefit.benefit.elegibleEmployees ?? "todos");

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Debug.WriteLine("Error en CreateBenefit: " + ex.Message);
                        return false;
                    }
                    finally
                    {
                        _connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error general en CreateBenefit: " + ex.Message);
                _connection.Close();
                return false;
            }
        }
    }
}
