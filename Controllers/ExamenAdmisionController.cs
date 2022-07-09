using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]    
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger;
        private readonly IMapper Mapper;
        public ExamenAdmisionController (KalumDbContext _dbContext, ILogger<ExamenAdmisionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ExamenAdmisionGetDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso en la base de datos");

            List<ExamenAdmision> examenes = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).ToListAsync();

            if(examenes == null || examenes.Count == 0)
            {
                Logger.LogWarning("No existe Examenes de Admision");
                return new NoContentResult();
            }
            List<ExamenAdmisionGetDTO> resumen = Mapper.Map<List<ExamenAdmisionGetDTO>>(examenes);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa.");
            return Ok(resumen);
        }

        [HttpGet("{id}", Name="GetExamenAdmision")]
        public async Task<ActionResult<ExamenAdmision>> GetExamenAdmision(string id)
        {
            Logger.LogDebug("Iniiciando proceso en la base de datos id " + id);
            var examenes = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if(examenes == null)
            {
                Logger.LogWarning("No existe el examenAdmision con el ID "+ id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa.");
            return Ok(examenes);
        }
        [HttpPost]
        public async Task<ActionResult<ExamenAdmision>> Post([FromBody] ExamenAdmisionCreateDTO value)
        {
            Logger.LogDebug("Iniciando proceso de agregar un examen de admision");

            ExamenAdmision nuevo = Mapper.Map<ExamenAdmision>(value);

            nuevo.ExamenId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.ExamenAdmision.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();

            Logger.LogInformation("Finalizando proceso de agregar un examen de admision");

            return new CreatedAtRouteResult("GetExamenAdmision",new{id = nuevo.ExamenId});
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ExamenAdmision value)
        {
            Logger.LogDebug("Iniciando el proceso de actualizacion de datos");

            ExamenAdmision actualizacion = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if(actualizacion == null)
            {
                Logger.LogWarning($"No existe la carrera tecnica con el {id}");
                return BadRequest();
            }
            actualizacion.FechaExamen = value.FechaExamen;
            DbContext.Entry(actualizacion).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Los datos del {id} han sido actualizados correctamente");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ExamenAdmision>> Delete(string id)
        {
            ExamenAdmision borrar = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if(borrar == null)
            {

                Logger.LogWarning("No existe la jornada con el ID " + id);

                return NotFound();
            }
            DbContext.ExamenAdmision.Remove(borrar);
            await DbContext.SaveChangesAsync();
            
            Logger.LogInformation($"Se ha eliminado correctamente la carrera tecnica con el id {id}");

            return NoContent();
        }
    }
}