using System.Data;
using back_end.Models;

using Microsoft.Data.SqlClient;

namespace back_end.Repositories
{
    public class ProfileRepository
    {
        private readonly SqlConnection _connection;
        private readonly string? _pathConnection;

        public ProfileRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _pathConnection = builder.Configuration.GetConnectionString("InfinipayDBContext");
            _connection = new SqlConnection(_pathConnection);
        }

        public ProfileModel GetProfileModel(ProfileModel profileModel, string tablaPersonaId)
        {
            string empresaId = "";
            if (profileModel.Rol == "empleador")
            {

            }
            else
            {

            }

            return profileModel;
        }






        private DataTable CrearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, _connection);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            _connection.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            _connection.Close();
            return consultaFormatoTabla;
        }
    }
}
