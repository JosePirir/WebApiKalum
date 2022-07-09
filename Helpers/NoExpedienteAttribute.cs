using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace WebApiKalum.Helpers
{
    public class NoExpedienteAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if(string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            if(value.ToString().Contains("-"))/*ve que el expediente tenga un -*/
            {
                int guion = value.ToString().IndexOf("-");/*donde se encuentra el guion*/
                string exp = value.ToString().Substring(0,guion);/*desde el inicio hasta, la cantidad*/
                 string numero = value.ToString().Substring(guion+1, value.ToString().Length - 4);
                
                if(!exp.ToUpper().Equals("EXP") || !Information.IsNumeric(numero))
                {
                    return new ValidationResult("El numero de expediente no contiene la nomenclatura adecuada");
                }
            }
            else
            {
                return new ValidationResult("El numero de expediente no tiene '-'");
            }
            return ValidationResult.Success;
        }
    }
}