using System.ComponentModel.DataAnnotations;

namespace WebApiKalum.Dtos
{
    public class CarreraTecnicaCreateDTO
    {
        [StringLength(128, MinimumLength = 5,ErrorMessage = "La cantidad minima de caracteres es {2} y la maxima es de {1} para el campo {0}")]/*Limitar cantidad de caracteres*/
        public string Nombre { get; set; }
    }
}