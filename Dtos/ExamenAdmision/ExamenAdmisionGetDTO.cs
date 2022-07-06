namespace WebApiKalum.Dtos
{
    public class ExamenAdmisionGetDTO
    {
        public string ExamenId { get; set; }
        public DateTime FechaExamen { get; set; }
        public virtual List<Aspirante_JornadaListDTO> Aspirantes { get; set; } /*se llama a la lista foranea*/ 

    }
}