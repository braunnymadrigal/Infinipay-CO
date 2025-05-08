using back_end.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
namespace back_end.Repositories
{
    public class BenefitRepository
    {
        private readonly string _connectionRoute;

        private void mapBenefit(BenefitModel benefit, DataRow row)
        {
            benefit.BenefitId = row["id"] != DBNull.Value ? (Guid?)row["id"] : null;
            benefit.BenefitName = row["nombre"] != DBNull.Value ? (string)row["nombre"] : null;
            benefit.BenefitMinTime = row["tiempoMinimo"] != DBNull.Value ? (decimal?)row["tiempoMinimo"] : null;
            benefit.BenefitDescription = row["descripcion"] != DBNull.Value ? (string)row["descripcion"] : null;
            benefit.BenefitElegibleEmployees = row["empleadoElegible"] != DBNull.Value ? (string)row["empleadoElegible"] : null;
            benefit.FormulaType = row["tipoFormula"] != DBNull.Value ? Convert.ToString(row["tipoFormula"]) : null;
            benefit.formulaParamUno = row["paramUno"] != DBNull.Value ? (string)row["paramUno"] : null;
            benefit.formulaParamDos = row["paramDos"] != DBNull.Value ? (string)row["paramDos"] : null;
            benefit.formulaParamTres = row["paramTres"] != DBNull.Value ? (string)row["paramTres"] : null;
            benefit.UserCreator = row["usuarioCreador"] != DBNull.Value ? (string)row["usuarioCreador"] : null;
            benefit.urlAPI = row["urlAPI"] != DBNull.Value ? (string)row["urlAPI"] : null;
            benefit.CreationDate = row["fechaCreacion"] != DBNull.Value ? (DateTime?)row["fechaCreacion"] : null;
            benefit.UserModifier = row["ultimoUsuarioModificador"] != DBNull.Value ? (string)row["ultimoUsuarioModificador"] : null;
            benefit.ModifiedDate = row["ultimaFechaModificacion"] != DBNull.Value ? (DateTime?)row["ultimaFechaModificacion"] : null;
        }

        public BenefitRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionRoute = builder.Configuration.GetConnectionString("InfinipayDBContext");
        }
        private SqlConnection GetConnection() {
            return new SqlConnection(_connectionRoute);
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
        private DataTable getQueryTable(string query, Dictionary<string, object> parameters)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                using (var adapter = new SqlDataAdapter(command))
                {
                    var table = new DataTable();
                    connection.Open();
                    adapter.Fill(table);
                    return table;
                }
            }
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
        public List<BenefitModel> GetAllBenefits(string nickname)
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
WHERE u.nickname = @nickname;;
    ";

            var parameters = new Dictionary<string, object>
    {
        { "@nickname", nickname },
    };

            var table = getQueryTable(query, parameters);
            var benefits = new List<BenefitModel>();

            foreach (DataRow row in table.Rows)
            {
                var benefit = new BenefitModel();
                mapBenefit(benefit, row);
                benefits.Add(benefit);
            }

            return benefits;
        }


        //public BenefitModel GetBenefitById(Guid idEmpresa, Guid id)
        //{
        //    var query = $"SELECT * FROM Beneficio WHERE IdBeneficio = {id}";
        //    var table = getQueryTable(query);
        //    if (table.Rows.Count == 0) return null;
        //    var row = table.Rows[0];
        //    var benefit = new BenefitModel();
        //    mapBenefit(benefit, row);
        //    return benefit;
        //}
        public bool CreateBenefit(BenefitModel benefit)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        
                        var auditId = insertAudit(benefit.UserNickname, transaction);

                        var formulaId = insertFormula(benefit, transaction);

                        var getEmpresaIdQuery = @"
                    SELECT pj.id AS idPersonaJuridica
                    FROM Usuario u
                    JOIN Empleador e ON u.idPersonaFisica = e.idPersonaFisica
                    JOIN PersonaJuridica pj ON e.idPersonaJuridica = pj.id
                    WHERE u.nickname = @nickName;";
                        SqlCommand cmd = new SqlCommand(getEmpresaIdQuery, connection, transaction);
                        cmd.Parameters.AddWithValue("@nickName", benefit.UserNickname);
                        var idPersonaJuridica = (Guid)cmd.ExecuteScalar();

                        var benefitId = insertBenefit(benefit, idPersonaJuridica, auditId, transaction);

                        insertDeduccion(benefit, idPersonaJuridica, benefitId, formulaId, auditId, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Debug.WriteLine("Error en CreateBenefit: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        private Guid insertAudit(string user, SqlTransaction transaction)
        {
            var cmd = new SqlCommand(@"
        INSERT INTO Auditoria (id, usuarioCreador, ultimoUsuarioModificador, ultimaFechaModificacion)
        OUTPUT INSERTED.id
        VALUES (NEWID(), @userNickName, @userNickName, SYSDATETIME());", transaction.Connection, transaction);

            cmd.Parameters.AddWithValue("@userNickName", user);
            return (Guid)cmd.ExecuteScalar();
        }

        private Guid insertFormula(BenefitModel benefit, SqlTransaction transaction)
        {
            var cmd = new SqlCommand(@"
        INSERT INTO Formula (id, tipoFormula, urlAPI, paramUno, paramDos, paramTres)
        OUTPUT INSERTED.id
        VALUES (NEWID() ,@TipoFormula, @UrlAPI, @ParamUno, @ParamDos, @ParamTres);", transaction.Connection, transaction);

            cmd.Parameters.AddWithValue("@TipoFormula", benefit.FormulaType ?? "");
            cmd.Parameters.AddWithValue("@UrlAPI", (object?)benefit.urlAPI ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ParamUno", benefit.formulaParamUno ?? "");
            cmd.Parameters.AddWithValue("@ParamDos", (object?)benefit.formulaParamDos ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ParamTres", (object?)benefit.formulaParamTres ?? DBNull.Value);
            return (Guid)cmd.ExecuteScalar();
        }

        private Guid insertBenefit(BenefitModel benefit, Guid empresaId, Guid auditId, SqlTransaction transaction)
        {
            var cmd = new SqlCommand(@"
        INSERT INTO Beneficio (id, nombre, tiempoMinimo, descripcion, empleadoElegible, idPersonaJuridica, idAuditoria)
        OUTPUT INSERTED.id
        VALUES (NEWID(), @nombre, @TiempoMinimo, @Descripcion, @Elegible, @IdPersonaJuridica, @IdAuditoria);", transaction.Connection, transaction);

            cmd.Parameters.AddWithValue("@Nombre", benefit.BenefitName ?? "");
            cmd.Parameters.AddWithValue("@TiempoMinimo", benefit.BenefitMinTime ?? 0);
            cmd.Parameters.AddWithValue("@Descripcion", benefit.BenefitDescription ?? "");
            cmd.Parameters.AddWithValue("@Elegible", benefit.BenefitElegibleEmployees ?? "todos");
            cmd.Parameters.AddWithValue("@IdPersonaJuridica", empresaId);
            cmd.Parameters.AddWithValue("@IdAuditoria", auditId);
            return (Guid)cmd.ExecuteScalar();
        }

        private void insertDeduccion(BenefitModel benefit, Guid empresaId, Guid benefitId, Guid formulaId, Guid auditId, SqlTransaction transaction)
        {
            var cmd = new SqlCommand(@"
        INSERT INTO Deduccion (nombre, descripcion, idPersonaJuridica, idBeneficio, idFormula, idAuditoria)
        VALUES (@Nombre, @Descripcion, @IdPersonaJuridica, @IdBeneficio, @IdFormula, @IdAuditoria);", transaction.Connection, transaction);

            cmd.Parameters.AddWithValue("@Nombre", benefit.BenefitName ?? "");
            cmd.Parameters.AddWithValue("@Descripcion", benefit.BenefitDescription ?? "");
            cmd.Parameters.AddWithValue("@IdPersonaJuridica", empresaId);
            cmd.Parameters.AddWithValue("@IdBeneficio", benefitId);
            cmd.Parameters.AddWithValue("@IdFormula", formulaId);
            cmd.Parameters.AddWithValue("@IdAuditoria", auditId);
            cmd.ExecuteNonQuery();
        }






    }
}
