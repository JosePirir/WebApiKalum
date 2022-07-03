namespace WebApiKalum.Dtos
{
    public class CarreraTecnicaListDTO
    {
        public string CarreraId { get; set; }
        public string Nombre { get; set; }
        public List<Aspirante_CarreraTecnicaListDTO> Aspirantes { get; set; }
        public List<Inscripcion_CarreraTecnicaListDTO> Inscripciones { get; set; }
    }
}