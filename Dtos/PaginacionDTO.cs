namespace WebApiKalum.Dtos
{
    public class PaginacionDTO<T>
    {
        public int Number {get; set;}
        public int TotalPages {get; set;}
        public bool First {get; set;}
        public bool Last {get; set;}/*esto servirá para la paginación*/
        public List<T> Content {get; set;} /*no lleva un tipo de dato especifico*/
        /*response acopla los registros*/
    }
}