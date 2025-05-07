using back_end.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Microsoft.Data.SqlClient;
namespace back_end.Repositories
{
    public class BenefitRepository
    {
        private readonly string _connectionRoute;

        private void mapBenefit(BenefitModel benefit, DataRow row)
        {
        
            benefit.BenefitId = row["IdBeneficio"] != DBNull.Value ? (Guid?)row["IdBeneficio"] : null;
            benefit.BenefitName = row["Nombre"] != DBNull.Value ? (string)row["Nombre"] : null;
            benefit.BenefitMinTime = row["MesesMinimos"] != DBNull.Value ? (decimal?)row["MesesMinimos"] : null;
            benefit.BenefitDescription = row["Descripcion"] != DBNull.Value ? (string)row["Descripcion"] : null;
            benefit.BenefitElegibleEmployees = row["EmpleadoElegible"] != DBNull.Value ? (string)row["EmpleadoElegible"] : null;
            benefit.FormulaType = row["TipoDeduccion"] != DBNull.Value ? (string)row["TipoDeduccion"] : null;
            benefit.formulaParamUno = row["Parametro1"] != DBNull.Value ? (string)row["Parametro1"] : null;
            benefit.formulaParamDos = row["Parametro2"] != DBNull.Value ? (string)row["Parametro2"] : null;
            benefit.formulaParamTres = row["Parametro3"] != DBNull.Value ? (string)row["Parametro3"] : null;
            benefit.UserCreator = row["UsuarioCreador"] != DBNull.Value ? (string)row["UsuarioCreador"] : null;
            benefit.urlAPI = row["UrlAPI"] != DBNull.Value ? (string)row["UrlAPI"] : null;
            benefit.CreationDate = row["FechaCreacion"] != DBNull.Value ? (DateTime?)row["FechaCreacion"] : null;
            benefit.UserModifier = row["UsuarioModificador"] != DBNull.Value ? (string)row["UsuarioModificador"] : null;
            benefit.ModifiedDate = row["FechaModificacion"] != DBNull.Value ? (DateTime?)row["FechaModificacion"] : null;
        

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
        public List<BenefitModel> GetAllBenefits(LoginUserModel user)
        {
            var queryEmpresa = @"
        SELECT pj.id
        FROM Usuario u
        JOIN PersonaFisica pf ON u.idPersonaFisica = pf.id
        JOIN Empleador e ON pf.id = e.idPersonaFisica
        JOIN PersonaJuridica pj ON e.idPersonaJuridica = pj.id
        WHERE u.nickname = @nickname OR u.nickname = @correo;
        ";

            var empresaIdTable = getQueryTable(queryEmpresa, new Dictionary<string, object>
            {
                { "@nickname", user.NicknameOrEmail },
                { "@correo", user.NicknameOrEmail }
            });

            if (empresaIdTable.Rows.Count == 0)
                return new List<BenefitModel>();

            var empresaId = (Guid)empresaIdTable.Rows[0]["id"];

            var queryBeneficios = @"
            SELECT * FROM Beneficio
            WHERE idPersonaJuridica = @empresaId
            ";

            var table = getQueryTable(queryBeneficios, new Dictionary<string, object>
            {
                { "@empresaId", empresaId }
            });

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

                var insertAuditoriaQuery = @"
            INSERT INTO Auditoria (usuarioCreador, ultimaFechaModificacion, ultimoUsuarioModificador)
            OUTPUT INSERTED.id
            VALUES (@UsuarioCreador, SYSDATETIME(), @UsuarioModificador);";

                Guid idAuditoria;
                using (var cmd = new SqlCommand(insertAuditoriaQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@UsuarioCreador", benefit.UserCreator ?? "");
                    cmd.Parameters.AddWithValue("@UsuarioModificador", benefit.UserCreator ?? "");
                    idAuditoria = (Guid)cmd.ExecuteScalar();
                }

                var insertFormulaQuery = @"
            INSERT INTO Formula (tipoFormula, urlAPI, paramUno, paramDos, paramTres)
            OUTPUT INSERTED.id
            VALUES (@TipoFormula, @UrlAPI, @ParamUno, @ParamDos, @ParamTres);";

                Guid idFormula;
                using (var cmd = new SqlCommand(insertFormulaQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@TipoFormula", benefit.FormulaType ?? "");
                    cmd.Parameters.AddWithValue("@UrlAPI", (object?)benefit.urlAPI ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ParamUno", benefit.formulaParamUno ?? "");
                    cmd.Parameters.AddWithValue("@ParamDos", (object?)benefit.formulaParamDos ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ParamTres", (object?)benefit.formulaParamTres ?? DBNull.Value);
                    idFormula = (Guid)cmd.ExecuteScalar();
                }

                Guid idPersonaJuridica;
                var getIdPersonaJuridicaQuery = @"
            SELECT ej.idPersonaJuridica
            FROM Usuario u
            JOIN Empleador e ON u.idPersonaFisica = e.idPersonaFisica
            JOIN PersonaJuridica ej ON e.idPersonaJuridica = ej.id
            WHERE u.idPersonaFisica = @IdPersonaFisica;";

                using (var cmd = new SqlCommand(getIdPersonaJuridicaQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@IdPersonaFisica", benefit.UserCreator);
                    idPersonaJuridica = (Guid)cmd.ExecuteScalar();
                }

                var insertBenefitQuery = @"
            INSERT INTO Beneficio (nombre, tiempoMinimo, descripcion, empleadoElegible, idPersonaJuridica, idAuditoria)
            OUTPUT INSERTED.id
            VALUES (@Nombre, @TiempoMinimo, @Descripcion, @Elegible, @IdPersonaJuridica, @IdAuditoria);";

                Guid idBeneficio;
                using (var cmd = new SqlCommand(insertBenefitQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", benefit.BenefitName ?? "");
                    cmd.Parameters.AddWithValue("@TiempoMinimo", benefit.BenefitMinTime ?? 0);
                    cmd.Parameters.AddWithValue("@Descripcion", benefit.BenefitDescription ?? "");
                    cmd.Parameters.AddWithValue("@Elegible", benefit.BenefitElegibleEmployees ?? "todos");
                    cmd.Parameters.AddWithValue("@IdPersonaJuridica", idPersonaJuridica);
                    cmd.Parameters.AddWithValue("@IdAuditoria", idAuditoria);
                    idBeneficio = (Guid)cmd.ExecuteScalar();
                }

                // Insertar Deducción
                var insertDeduccionQuery = @"
            INSERT INTO Deduccion (nombre, descripcion, idPersonaJuridica, idBeneficio, idFormula, idAuditoria)
            VALUES (@Nombre, @Descripcion, @IdPersonaJuridica, @IdBeneficio, @IdFormula, @IdAuditoria);";

                using (var cmd = new SqlCommand(insertDeduccionQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", benefit.BenefitName ?? "");
                    cmd.Parameters.AddWithValue("@Descripcion", benefit.BenefitDescription ?? "");
                    cmd.Parameters.AddWithValue("@IdPersonaJuridica", idPersonaJuridica);
                    cmd.Parameters.AddWithValue("@IdBeneficio", idBeneficio);
                    cmd.Parameters.AddWithValue("@IdFormula", idFormula);
                    cmd.Parameters.AddWithValue("@IdAuditoria", idAuditoria);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }




    }
}
