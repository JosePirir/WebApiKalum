namespace WebApiKalum.Dtos
{
    public class JornadaListDTO
    {
        public string JornadaId { get; set; }
        public string JornadaNombre { get; set;}
        public string Descripcion { get; set; }
        public virtual List<Aspirante_JornadaListDTO> Aspirantes { get; set;}
        public virtual List<Inscripcion_CarreraTecnicaListDTO> Inscripciones { get; set; }
    }
}