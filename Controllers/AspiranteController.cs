using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [Route("v1/KalumManagement/[controller]")]
    [ApiController] /*estas son etiquetas*/
    public class AspiranteController: ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AspiranteController> Logger;
        private readonly IMapper Mapper;
        public AspiranteController(KalumDbContext _dbContext, ILogger<AspiranteController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        [ServiceFilter(typeof(ActionFilter))]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> Get()
        {
            Logger.LogDebug("Iniciando proceso de consulta de aspirante");
            List<Aspirante> lista = await DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).Include(a => a.ExamenAdmision).ToListAsync();
            if(lista == null || lista.Count == 0)
            {
                Logger.LogWarning("No existen registros en la base de datos");
                return new NoContentResult();
            }
            List<AspiranteListDTO> aspirantes = Mapper.Map<List<AspiranteListDTO>>(lista);
            Logger.LogInformation("La consulta se ejecuto con exito");
            return Ok(aspirantes);
        }

        [HttpGet("{id}", Name="GetAspirante")]
        public async Task<ActionResult<Aspirante>> GetAspirante(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda del id " + id);
            var aspirante = await DbContext.Aspirante.Include(a => a.Jornada).Include(a => a.CarreraTecnica).Include(a => a.ExamenAdmision).FirstOrDefaultAsync(a => a.NoExpediente == id);
            if(aspirante == null)
            {
                Logger.LogWarning("No existe el aspirante con el ID: "+id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizado el proceso de busqueda de manera exitosa");
            return Ok(aspirante);
        }

        [HttpPost]
        public async Task<ActionResult<Aspirante>> Post([FromBody] Aspirante value)/*viene del cuerpo de la peticion la informacion*//*detipo action result de tipo aspirante*/
        {
            Logger.LogDebug("Iniciando proceso para almacenar un registro de aspirante");

            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if(carreraTecnica == null)
            {
                Logger.LogInformation($"No existe la carreraTecnica con el id{value.CarreraId}");
                return BadRequest();/*error 400 de parte del usuario*/
            }

            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == value.JornadaId);
            if(jornada == null)
            {
                Logger.LogInformation($"No existe la jornada con el id {value.JornadaId}");
            }

            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(e => e.ExamenId == value.ExamenId);
            if(examenAdmision == null)
            {
                Logger.LogInformation($"No existe el examen de admision con el id {value.ExamenId}");
                return BadRequest();
            }

            await DbContext.Aspirante.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se a creado el aspirante con exito");
            return Ok(value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] AspirantePutDTO value)
        {//Carrera Examen y Jornada
            Logger.LogDebug("Iniciando el proceso de actualización de datos");

            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(e => e.NoExpediente == id);
            if(aspirante == null)
            {
                Logger.LogWarning($"No existe el aspirante con el NoExpediente {id}");
                return BadRequest();
            }
            aspirante.Nombres = value.Nombres;
            aspirante.Apellidos = value.Apellidos;
            DbContext.Entry(aspirante).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Aspirante>> Delete(string id)
        {
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == id);
            if(aspirante == null)
            {
                return NotFound();
            }
            DbContext.Aspirante.Remove(aspirante);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se ha eliminada el aspirante con el No. de expediente {id}");
            return NoContent();
        }
    }
}