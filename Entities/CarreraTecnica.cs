using System.ComponentModel.DataAnnotations;
using WebApiKalum.Entities;

namespace WebApiKalum.Entities
{
    public class CarreraTecnica
    {
        [Required]
        public string CarreraId { get; set; }
        [Required (ErrorMessage = "El campo {0} es requerido")]
        [StringLength(128, MinimumLength = 5,ErrorMessage = "La cantidad minima de caracteres es {2} y la maxima es de {1} para el campo {0}")]/*Limitar cantidad de caracteres*/
        public string Nombre { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; } /**/
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<InversionCarreraTecnica> InversionCarreraTecnica { get; set; }
    }
}
/*uno a muchos*/