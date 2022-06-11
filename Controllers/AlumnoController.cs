using Microsoft.AspNetCore.Mvc; /*Api Controller y Route*/
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController] /*usará Http*/
    [Route("v1/KalumManagement/[controller]")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;
        public AlumnoController(KalumDbContext _dbContext, ILogger<AlumnoController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }

        [HttpGet]

        public async Task<ActionResult<List<Alumno>>> Get()
        {
            List<Alumno> alumnos = null;
            Logger.LogDebug("Iniciando proceso en la base de datos");

            alumnos = await DbContext.Alumno.Include(a2 => a2.Inscripcion).Include(a2=> a2.CuentaXCobrar).ToListAsync();

            if(alumnos == null || alumnos.Count == 0)
            {
                Logger.LogWarning("No existen alumnos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(alumnos);
        }

        [HttpGet("{id}", Name="GetAlumno")]

        public async Task<ActionResult<Alumno>> GetAlumno(string id)
        {
            Logger.LogDebug("Iniciando proceso en la base de datos "+id);
            var alumnos = await DbContext.Alumno.Include(a2 => a2.Inscripcion).Include(a2 => a2.CuentaXCobrar).FirstOrDefaultAsync(a2=>a2.Carne == id);
            if(alumnos == null)
            {
                Logger.LogWarning("No existe el alumno con el ID "+id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de bsuqueda de forma exitosa.");
            return Ok(alumnos);
        }
    }
}