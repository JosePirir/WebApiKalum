using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        public CarreraTecnicaController(KalumDbContext _dbContext, ILogger<CarreraTecnicaController> _Logger)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarreraTecnica>>> Get()
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            Logger.LogDebug("Iniciando procesos en la base de datos");
            //Tarea1
            carrerasTecnicas = await DbContext.CarreraTecnica.Include(c=>c.Aspirantes).Include(c=>c.Inscripciones).Include(c=>c.InversionCarreraTecnica).ToListAsync();

             //Tarea 2
            if(carrerasTecnicas == null || carrerasTecnicas.Count == 0)
            {
                Logger.LogWarning("No existe carreras tecnicas");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa.");
            return Ok(carrerasTecnicas);
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
        public async Task<ActionResult<CarreraTecnica>> Post([FromBody] CarreraTecnica value)
        {
            Logger.LogDebug("Iniciando proceso de agregar una carrera tecnica nueva");
            value.CarreraId = Guid.NewGuid().ToString().ToUpper(); /*crea un nuevo ID aleatorio*/
            await DbContext.CarreraTecnica.AddAsync(value);
            await DbContext.SaveChangesAsync();/* cuando termine este metodo se indica que ejecute las siguientes lineas de codigo con await*/
            Logger.LogInformation("Finalizando proceso de agregar una carrera tecnica nueva");
            return new CreatedAtRouteResult("GetCarreraTecnica",new{id=value.CarreraId}, value);/*parametros ("ruta", "id", "objeto" )*/ /*nuevo objeto a una ruta ya establecida*/
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CarreraTecnica>> Delete(string id)
        {
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id); /*Firstordefault es el primero que encuentre*/
            if(carreraTecnica == null)
            {
                return NotFound();
            }
            else
            {
                DbContext.CarreraTecnica.Remove(carreraTecnica);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la carrera tecnica con el id {id}");
                return carreraTecnica;
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
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }
    }
}