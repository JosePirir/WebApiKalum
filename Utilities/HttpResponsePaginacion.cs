using WebApiKalum.Dtos;

namespace WebApiKalum.Utilities
{
    public class HttpResponsePaginacion<T> : PaginacionDTO<T>
    {
        public HttpResponsePaginacion(IQueryable<T> source, int number) /*constructor que recibe*/
        {
            this.Number = number;
            int cantidadRegistrosPorPagina = 5;
            int totalRegistros = source.Count();/*se va ejecutar hasta */
            this.TotalPages = (int) Math.Ceiling((Double)totalRegistros/cantidadRegistrosPorPagina);/*contar la cantidad de paginas totales en int*/
            this.Content = source.Skip(cantidadRegistrosPorPagina * Number).Take(cantidadRegistrosPorPagina).ToList();
            if(this.Number == 0)
            {
                this.First = true;
            }
            else if ((this.Number + 1) == this.TotalPages) /*para que haga match con cantidad de registros sin error*/ /*se suma + 1 porque se empieza desde 0*/
            {
                this.Last = true;
            }
        }
    }
}