using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class CargoCreateDTO
    {
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public string Prefijo { get; set; }
        [Required]
        public decimal Monto { get; set; }
        [Required]
        public bool GeneraMora { get; set; }
        [Required]
        public int PorcentajeMora { get; set; }
    }
}