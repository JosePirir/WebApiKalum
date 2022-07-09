using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using WebApiKalum.Helpers;

namespace WebApiKalum.Entities
{
    public class Aspirante //: IValidatableObject
    {
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(12, MinimumLength = 11, ErrorMessage =" El campo número de expediente debe de ser de 12 caracteres")]
        [NoExpediente]
        public string NoExpediente {get; set;}
        [Required(ErrorMessage ="El campo {0} es requerido")]
        public string Apellidos {get; set;}
        [Required(ErrorMessage ="El campo {0} es requerido")]
        public string Nombres {get; set;}
        [Required(ErrorMessage ="El campo {0} es requerido")]
        public string Direccion {get; set;}
        [Required(ErrorMessage ="El campo {0} es requerido")]
        public string Telefono {get; set;}
        [Required(ErrorMessage ="El campo {0} es requerido")]
        public string Email {get; set;}
        public string Estatus {get; set;} = "NO ASIGNADO";
        public string CarreraId {get; set;}
        public string JornadaId {get; set;}
        public string ExamenId {get; set;}
        public virtual CarreraTecnica CarreraTecnica {get; set;}/*Que traiga los aspirantes *//*estos son llaves foraneas*/
        public virtual Jornada Jornada {get; set;}/*si no se pone lista solo llama a uno*//*estos son llaves foraneas*/
        public virtual ExamenAdmision ExamenAdmision {get; set;}/*estos son llaves foraneas*/
        public virtual List<ResultadoExamenAdmision> ResultadoExamenAdmision {get; set;}/*estos son llaves foraneas de ResultadoExamenAdmision*/
        public virtual List<InscripcionPago> InscripcionPago { get; set; }/*estos son llaves foraneas de InscripcionPago*/

//       public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)/*funcion necesita un objeto de tipo ValidationConetext*/
//      {  
//                int guion = NoExpediente.IndexOf("-");/*donde se encuentra el guion*/
//                string exp = NoExpediente.Substring(0,guion);/*desde el inicio hasta, la cantidad*/
 //               string numero = NoExpediente.Substring(guion+1, NoExpediente.Length - 4);
     //           if(!NoExpediente.Contains("-"))/*ve que el expediente tenga un -*/
 //               {
    //                yield return new ValidationResult("El numero de expediente es invalido, no contiene un -", new string[]{nameof(NoExpediente)});/*se crea una nueva lista (vector) con el valor de NoExpediente*/
  //              }
       //         if(!exp.ToUpper().Equals("EXP") || !Information.IsNumeric(numero))
         //       {
         //           yield return new ValidationResult("El numero de expediente no contiene la nomenclatura adecuada", new string[]{nameof(NoExpediente)});
          //      }
   //     }
        
    }
}/*muchos a uno*/