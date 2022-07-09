using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class JornadaCreateDTO
    {
        [Required]
        [StringLength(2, ErrorMessage = "La cantidad maxima de caracteres es {1} para el campo {0}")]
        public string JornadaNombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
    }
}