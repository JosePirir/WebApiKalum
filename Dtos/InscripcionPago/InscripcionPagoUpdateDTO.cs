using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class InscripcionPagoUpdateDTO
    {
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
    }
}