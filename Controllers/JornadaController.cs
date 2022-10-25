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
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;
        private readonly IMapper Mapper;
        public JornadaController(KalumDbContext _dbContext, ILogger<JornadaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<JornadaListDTO>>> Get()
        {
            List<Jornada> jornadas = null;
            Logger.LogDebug("Iniciando proceso en la base de datos");

            jornadas = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j=>j.Inscripciones).ToListAsync();

            if(jornadas == null || jornadas.Count == 0)
            {
                Logger.LogWarning("No existe jornadas");
                return new NoContentResult();
            }
            List<JornadaListDTO> jornadaListDTO = Mapper.Map<List<JornadaListDTO>>(jornadas);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa.");
            return Ok(jornadaListDTO);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<Jornada>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.Jornada.Include(j => j.Aspirantes).Include(j => j.Inscripciones).AsQueryable();/*se va a ejectuar despues*/
            var paginacion = new HttpResponsePaginacion<Jornada>(queryable,page);
            if(paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen Jornadas");
                return NoContent();
            }
            else
            {
                return Ok(paginacion);
            }
        }

        [HttpGet("{id}", Name="GetJornada")]
        public async Task<ActionResult<Jornada>> GetJornada(string id) /*resultado*/
        {
            Logger.LogDebug("Iniciando proceso en la base de datos id " + id);
            var jornada = await DbContext.Jornada.Include(j=>j.Aspirantes).Include(j=>j.Inscripciones).FirstOrDefaultAsync(j=>j.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning("No existe la jornada con el ID " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa.");
            return Ok(jornada);
        }

        [HttpPost]
        public async Task<ActionResult<Jornada>> Post([FromBody] JornadaCreateDTO value)
        {
            Logger.LogDebug("Iniciando proceso de agregar Jornada");
            Jornada nuevaJornada = Mapper.Map<Jornada>(value);
            nuevaJornada.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(nuevaJornada);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de crear una jornada.");
            return new CreatedAtRouteResult("GetJornada",new{id=nuevaJornada.JornadaId}, nuevaJornada);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Jornada>> Delete(string id)
        {
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning("No existe la jornada con el ID " + id);
                return NotFound();
            }
                DbContext.Jornada.Remove(jornada);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la jornada con el id {id}");
                return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] JornadaCreateDTO value)
        {
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning($"No existe la jornada con el id {id}");
                return BadRequest();
            }
            jornada.JornadaNombre = value.JornadaNombre;
            jornada.Descripcion = value.Descripcion;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Los datos de la jornada con el id: {id} han sido actualizados.");
            return NoContent();
        }
    }  
}