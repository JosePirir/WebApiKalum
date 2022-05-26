using WebApiKalum.Entities;

namespace WebApiKalum.Entities
{
    public class Jornada
    {
        public string JornadaId { get; set; }
        public string JornadaNombre { get; set; }
        public string Descripcion { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
    }
}