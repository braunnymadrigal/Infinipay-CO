using back_end.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
namespace back_end.Repositories
{
    public class BeneficioRepository
    {
        private SqlConnection _conexion;
        private string _rutaConexion;
        public BeneficioRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion = builder.Configuration.GetConnectionString("PaisesContext");
            _conexion = new SqlConnection(_rutaConexion);
        }
        private DataTable CrearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _conexion);
            SqlDataAdapter adaptadorParaTabla = new
            SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            _conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            _conexion.Close();
            return consultaFormatoTabla;
        }
        public List<BeneficioModel> ObtenerBeneficios()
        {
            List<BeneficioModel> beneficios = new List<BeneficioModel>();
            string consulta = "SELECT * FROM dbo.Beneficio";
            DataTable tablaResultado = CrearTablaConsulta(consulta);

            foreach (DataRow fila in tablaResultado.Rows)
            {
                beneficios.Add(
                    new BeneficioModel
                    {
                        Id = Guid.Parse(fila["id"].ToString()),
                        Nombre = fila["nombre"].ToString(),
                        TiempoMinimo = Convert.ToDecimal(fila["tiempoMinimo"]),
                        Descripcion = fila["descripcion"].ToString(),
                        EmpleadoElegible = fila["empleadoElegible"].ToString(),
                        IdPersonaJuridica = Guid.Parse(fila["idPersonaJuridica"].ToString()),
                        IdAuditoria = Guid.Parse(fila["idAuditoria"].ToString())
                    }
                );
            }

            return beneficios;
        }

        public bool CrearBeneficio(BeneficioModel beneficio)
        {
            var consulta = @"INSERT INTO [dbo].[Beneficio] 
        ([nombre], [tiempoMinimo], [descripcion], [empleadoElegible], [idPersonaJuridica], [idAuditoria])
        VALUES (@nombre, @tiempoMinimo, @descripcion, @empleadoElegible, @idPersonaJuridica, @idAuditoria)";

            var comando = new SqlCommand(consulta, _conexion);
            comando.Parameters.AddWithValue("@nombre", beneficio.Nombre);
            comando.Parameters.AddWithValue("@tiempoMinimo", beneficio.TiempoMinimo);
            comando.Parameters.AddWithValue("@descripcion", beneficio.Descripcion);
            comando.Parameters.AddWithValue("@empleadoElegible", beneficio.EmpleadoElegible);
            comando.Parameters.AddWithValue("@idPersonaJuridica", beneficio.IdPersonaJuridica);
            comando.Parameters.AddWithValue("@idAuditoria", beneficio.IdAuditoria);

            _conexion.Open();
            bool exito = comando.ExecuteNonQuery() >= 1;
            _conexion.Close();

            return exito;
        }


    }
}
