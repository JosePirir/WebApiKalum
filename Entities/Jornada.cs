using System.ComponentModel.DataAnnotations;
using WebApiKalum.Entities;

namespace WebApiKalum.Entities
{
    public class Jornada
    {
        [Required]
        public string JornadaId { get; set; }
        [StringLength(2, ErrorMessage = "La cantidad maxima de caracteres es {1} para el campo {0}")]
        public string JornadaNombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
    }
}