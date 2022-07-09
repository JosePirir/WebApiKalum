using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        private readonly IMapper Mapper; /*injectar objeto de tipo automaper para hacer la conversion*/
        public CarreraTecnicaController(KalumDbContext _dbContext, ILogger<CarreraTecnicaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarreraTecnicaListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando procesos en la base de datos");
            //Tarea1
            List<CarreraTecnica> list = await DbContext.CarreraTecnica.Include(c=>c.Aspirantes).Include(c=>c.Inscripciones).Include(c=>c.InversionCarreraTecnica).ToListAsync();

             //Tarea 2
            if(list == null || list.Count == 0)
            {
                Logger.LogWarning("No existe carreras tecnicas");
                return new NoContentResult();
            }
            List<CarreraTecnicaListDTO> carrerasTecnicas = Mapper.Map<List<CarreraTecnicaListDTO>>(list);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa.");
            return Ok(carrerasTecnicas);
        }

        [HttpGet("page/{page}")]/*get con paginación */
        public async Task<ActionResult<IEnumerable<CarreraTecnica>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.CarreraTecnica.Include(ct => ct.Aspirantes).Include(ct => ct.Inscripciones).AsQueryable();/*se va a ejectuar despues*/
            var paginacion = new HttpResponsePaginacion<CarreraTecnica>(queryable, page);
            if(paginacion.Content == null && paginacion.Content.Count == 0)/*se pone && porque si no tiene contenido no puede hacer un count y tira error*/
            {                
                Logger.LogWarning("No existe carreras tecnicas");
                return NoContent();
            }
            else
            {
                return Ok(paginacion);
            }
        }

        
        [HttpGet("{id}", Name="GetCarreraTecnica")]
        public async Task<ActionResult<CarreraTecnica>> GetCarreraTecnica(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var carrera = await DbContext.CarreraTecnica.Include(c=>c.Aspirantes).Include(c=>c.Inscripciones).Include(c=>c.InversionCarreraTecnica).FirstOrDefaultAsync(ct=>ct.CarreraId == id);
            if(carrera== null)
            {
                Logger.LogWarning("No existe la carrera tenica con el ID "+ id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(carrera);
        }

        [HttpPost]
        public async Task<ActionResult<CarreraTecnica>> Post([FromBody] CarreraTecnicaCreateDTO value) /*se recibe de tipo carreraTecnicaDTO*/
        {
            Logger.LogDebug("Iniciando proceso de agregar una carrera tecnica nueva");
            CarreraTecnica nuevo = Mapper.Map<CarreraTecnica>(value); /*convierte de tipo CarreraTecnicaDTO a tipo CarreraTecnica, hace match con entidades iguales*/

            nuevo.CarreraId = Guid.NewGuid().ToString().ToUpper(); /*crea un nuevo ID aleatorio*/
            await DbContext.CarreraTecnica.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();/* cuando termine este metodo se indica que ejecute las siguientes lineas de codigo con await*/
            Logger.LogInformation("Finalizando proceso de agregar una carrera tecnica nueva");
            return new CreatedAtRouteResult("GetCarreraTecnica",new{id=nuevo.CarreraId}, nuevo);/*parametros ("ruta", "id", "objeto" )*/ /*nuevo objeto a una ruta ya establecida*/
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CarreraTecnica>> Delete(string id)
        {
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id); /*Firstordefault es el primero que encuentre*/
            if(carreraTecnica == null)
            {
                Logger.LogWarning("No existe la jornada con el ID " + id);
                return NotFound();
            }
            else
            {
                DbContext.CarreraTecnica.Remove(carreraTecnica);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la carrera tecnica con el id {id}");
                return NoContent();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] CarreraTecnica value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion de datos");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carreraTecnica == null)
            {
                Logger.LogWarning($"No existe la carrera tecnica con el {id}");
                return BadRequest();
            }
            carreraTecnica.Nombre = value.Nombre;
            DbContext.Entry(carreraTecnica).State = EntityState.Modified; /*este objeto lo acabo de modificar ahora hace la actualizacion en la base de datos*/
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Los datos del {id} han sido actualizados correctamente");
            return NoContent();
        }
    }
}