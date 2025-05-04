namespace back_end.Models
{
    public class ProfileModel
    {
        public ProfileModel()
        {
            this.PrimerNombre = "";
            this.SegundoNombre = "";
            this.PrimerApellido = "";
            this.SegundoApellido = "";
            this.NombreUsuario = "";
            this.Cedula = "";
            this.Correo = "";
            this.Telefono = "";
            this.Provincia = "";
            this.Canton = "";
            this.Distrito = "";
            this.DireccionExacta = "";
            this.Genero = "";
            this.FechaNacimiento = "";
            this.Empresa = "";
            this.Rol = "";
        }

        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Cedula {  get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Provincia { get; set; }
        public string Canton {  get; set; }
        public string Distrito { get; set; }
        public string DireccionExacta { get; set; }
        public string Genero { get; set; }
        public string FechaNacimiento { get; set; }
        public string Empresa { get; set; }
        public string Rol {  get; set; }
    }
}
