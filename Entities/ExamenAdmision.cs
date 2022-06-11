using WebApiKalum.Entities;

namespace WebApiKalum.Entities
{
    public class ExamenAdmision
    {
        public string ExamenId { get; set; }
        public DateTime FechaExamen { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; } /*se llama a la lista foranea*/ 
    }
}