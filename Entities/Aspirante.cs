using WebApiKalum.Entities;

namespace WebApiKalum.Entities
{
    public class Aspirante
    {
        public string NoExpediente {get; set;}
        public string Apellidos {get; set;}
        public string Nombres {get; set;}
        public string Direccion {get; set;}
        public string Telefono {get; set;}
        public string Email {get; set;}
        public string Estatus {get; set;}
        public string CarreraId {get; set;}
        public string JornadaId {get; set;}
        public string ExamenId {get; set;}
        public virtual CarreraTecnica CarreraTecnica {get; set;}/*Que traiga los aspirantes *//*estos son llaves foraneas*/
        public virtual Jornada Jornada {get; set;}/*si no se pone lista solo llama a uno*//*estos son llaves foraneas*/
        public virtual ExamenAdmision ExamenAdmision {get; set;}/*estos son llaves foraneas*/
        public virtual List<ResultadoExamenAdmision> ResultadoExamenAdmision {get; set;}/*estos son llaves foraneas de ResultadoExamenAdmision*/
        public virtual List<InscripcionPago> InscripcionPago { get; set; }/*estos son llaves foraneas de InscripcionPago*/
    }
}/*muchos a uno*/