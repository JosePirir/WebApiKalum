using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]    
    public class ExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController> Logger;
        public ExamenAdmisionController (KalumDbContext _dbContext, ILogger<ExamenAdmisionController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<List<ExamenAdmision>>> Get()
        {
            List<ExamenAdmision> examenes = null;
            Logger.LogDebug("Iniciando proceso en la base de datos");

            examenes = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).ToListAsync();

            if(examenes == null || examenes.Count == 0)
            {
                Logger.LogWarning("No existe Examenes de Admision");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa.");
            return Ok(examenes);
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
    }
}