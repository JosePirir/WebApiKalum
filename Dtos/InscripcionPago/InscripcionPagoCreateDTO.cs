using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class InscripcionPagoCreateDTO
    {
        [Required]
        public string NoExpediente { get; set; }
        [Required]
        public string Anio { get; set; }
        [Required]
        public DateTime FechaPago { get; set; }
        [Required]
        public decimal Monto { get; set; }
    }
}