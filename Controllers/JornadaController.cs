using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{ 
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;
        public JornadaController(KalumDbContext _dbContext, ILogger<JornadaController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<List<Jornada>>> Get()
        {
            List<Jornada> jornadas = null;
            Logger.LogDebug("Iniciando proceso en la base de datos");

            jornadas = await DbContext.Jornada.Include(j => j.Aspirantes).Include(j=>j.Inscripciones).ToListAsync();

            if(jornadas == null || jornadas.Count == 0)
            {
                Logger.LogWarning("No existe jornadas");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa.");
            return Ok(jornadas);
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
    }  
}