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
            string consulta = "";
            string empleadorId = tablaPersonaId;
            if (profileModel.Rol != "empleador")
            {
                consulta = $"SELECT [idEmpleadorContratador] FROM [Empleado] WHERE [idPersonaFisica] = '{tablaPersonaId}';";
                empleadorId = GetStringCol1Fila1(consulta);
            }
            consulta = $"SELECT [idPersonaJuridica] FROM [Empleador] WHERE [idPersonaFisica] = '{empleadorId}';";
            string empresaId = GetStringCol1Fila1(consulta);
            consulta = $"SELECT * FROM [PersonaJuridica] WHERE [id] = '{empresaId}';";
            profileModel = LlenarEmpresa(profileModel, consulta);
            consulta = $"SELECT * FROM [Direccion] WHERE [idPersona] = '{tablaPersonaId}';";
            profileModel = LlenarDireccion(profileModel, consulta);
            consulta = $"SELECT * FROM [PersonaFisica] WHERE [id] = '{tablaPersonaId}';";
            profileModel = LlenarPersonaFisica(profileModel, consulta);
            return profileModel;
        }


        private ProfileModel LlenarPersonaFisica(ProfileModel profileModel, string consulta)
        {
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                DataRow filaResultado = tablaResultado.Rows[0];
                var primerNombre = Convert.ToString(filaResultado["primerNombre"]);
                var segundoNombre = Convert.ToString(filaResultado["segundoNombre"]);
                var primerApellido = Convert.ToString(filaResultado["primerApellido"]);
                var segundoApellido = Convert.ToString(filaResultado["segundoApellido"]);
                var genero = Convert.ToString(filaResultado["genero"]);
                if (primerNombre != null && segundoNombre != null && primerApellido != null 
                    && segundoApellido != null && genero != null)
                {
                    profileModel.PrimerNombre = primerNombre;
                    profileModel.SegundoNombre= segundoNombre;
                    profileModel.PrimerApellido= primerApellido;
                    profileModel.SegundoApellido= segundoApellido;
                    profileModel.Genero = genero;
                }
            }
            return profileModel;
        }

        private ProfileModel LlenarDireccion(ProfileModel profileModel, string consulta)
        {
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                DataRow filaResultado = tablaResultado.Rows[0];
                var provincia = Convert.ToString(filaResultado["provincia"]);
                var canton = Convert.ToString(filaResultado["canton"]);
                var distrito = Convert.ToString(filaResultado["distrito"]);
                var otrasSenas = Convert.ToString(filaResultado["otrasSenas"]);
                if (provincia != null && canton != null && distrito != null && otrasSenas != null)
                {
                    profileModel.Provincia = provincia;
                    profileModel.Canton = canton;
                    profileModel.Distrito = distrito;
                    profileModel.DireccionExacta = otrasSenas;
                }
            }
            return profileModel;
        }

        private ProfileModel LlenarEmpresa(ProfileModel profileModel, string consulta)
        {
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                DataRow filaResultado = tablaResultado.Rows[0];
                var razonSocial = Convert.ToString(filaResultado["razonSocial"]);
                if (razonSocial != null)
                {
                    profileModel.Empresa = razonSocial;
                }
            }
            return profileModel;
        }

        private string GetStringCol1Fila1(string consulta)
        {
            string returnString = "";
            DataTable tablaResultado = CrearTablaConsulta(consulta);
            if (tablaResultado.Rows.Count > 0)
            {
                DataRow filaResultado = tablaResultado.Rows[0];
                var valorCol1Fil1 = Convert.ToString(filaResultado[0]);
                if (valorCol1Fil1 != null)
                {
                    returnString = valorCol1Fil1;
                }
            }
            return returnString;
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
