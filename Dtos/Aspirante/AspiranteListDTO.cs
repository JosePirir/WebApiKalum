namespace WebApiKalum.Dtos
{
    public class AspiranteListDTO
    {
        public string NoExpediente { get; set; }
        public string NombreCompleto { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public CarreraTecnicaCreateDTO CarreraTecnica { get; set; } /*entities amarrado a los modelo de datos a las tablas de la base de Datos deben llevar virtual*/
        public JornadaCreateDTO Jornada { get; set; } /*y los DTOS se personalizan pero no van a la base de datos*/
        public ExamenAdmisionCreateDTO ExamenAdmision { get; set; }
    }
}