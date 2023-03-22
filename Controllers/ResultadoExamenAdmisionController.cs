using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Utilities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class ResultadoExamenAdmisionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ResultadoExamenAdmisionController> Logger;
        private readonly IMapper Mapper;
        public ResultadoExamenAdmisionController (KalumDbContext _dbContext, ILogger<ResultadoExamenAdmisionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultadoExamenAdmisionListDTO>>> Get()
        {
            List<ResultadoExamenAdmision> resultados = await DbContext.ResultadoExamenAdmision.Include(rea => rea.Aspirantes).ToListAsync();
            if(resultados == null || resultados.Count == 0)
            {
                return new NoContentResult();
            }
            List<ResultadoExamenAdmisionListDTO> resumen = Mapper.Map<List<ResultadoExamenAdmisionListDTO>>(resultados);
            return Ok(resumen);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<ResultadoExamenAdmision>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.ResultadoExamenAdmision.Include(rea => rea.Aspirantes).AsQueryable();
            var paginacion = new HttpResponsePaginacion<ResultadoExamenAdmision>(queryable,page);
            if(paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen Resultados de Examenes de admisión");
                return NoContent();
            }
            else
            {
                return Ok(paginacion);
            }
        }
        
        [HttpGet("{id}", Name="GetResultadoExamenAdmision")]
        public async Task<ActionResult<ResultadoExamenAdmision>> GetResultadoExamenAdmision(string id)
        {
            var resultado = await DbContext.ResultadoExamenAdmision.Include(rea => rea.Aspirantes).FirstOrDefaultAsync(rea => rea.NoExpediente == id);
            if(resultado == null)
            {
                return new NoContentResult();
            }
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<ResultadoExamenAdmision>> Post([FromBody] ResultadoExamenAdmision value)
        {
            await DbContext.ResultadoExamenAdmision.AddAsync(value);
            await DbContext.SaveChangesAsync();
            return new CreatedAtRouteResult("GetResultadoExamenAdmision", new{id=value.NoExpediente}, value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ResultadoExamenAdmision value)
        {
            ResultadoExamenAdmision resultado = await DbContext.ResultadoExamenAdmision.FirstOrDefaultAsync(rea => rea.NoExpediente == id);
            if(resultado == null)
            {
                return BadRequest();
            }
            resultado.Descripcion = value.Descripcion;
            resultado.Nota = value.Nota;
            DbContext.Entry(resultado).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResultadoExamenAdmision>> Delete(string id)
        {
            ResultadoExamenAdmision resultado = await DbContext.ResultadoExamenAdmision.FirstOrDefaultAsync(rea => rea.NoExpediente == id);
            if(resultado == null)
            {
                return NotFound();
            }
            DbContext.ResultadoExamenAdmision.Remove(resultado);
            await DbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}