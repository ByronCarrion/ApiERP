namespace ApiERP.Models.ModelExtensions
{
    public class Trabajadores
    {
        public int Compania { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Dni { get; set; }
        public int? Activo { get; set; }
        public string FechaNacimiento { get; set; }
        public int Sexo { get; set; }
        public int Profesion { get; set; }
        public string Funcion { get; set; }
        public string Planta { get; set; }
    }
}