namespace back_end.Models
{
    public class ProfileModel
    {
        public ProfileModel()
        {
            this.PrimerNombre = "No especificado";
            this.SegundoNombre = "No especificado";
            this.PrimerApellido = "No especificado";
            this.SegundoApellido = "No especificado";
            this.NombreUsuario = "No especificado";
            this.Cedula = "No especificado";
            this.Correo = "No especificado";
            this.Telefono = "No especificado";
            this.Provincia = "No especificado";
            this.Canton = "No especificado";
            this.Distrito = "No especificado";
            this.DireccionExacta = "No especificado";
            this.Genero = "No especificado";
            this.FechaNacimiento = "No especificado";
            this.Empresa = "No especificado";
            this.Rol = "No especificado";
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
