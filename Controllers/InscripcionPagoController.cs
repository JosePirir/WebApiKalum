using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class InscripcionPagoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InscripcionPagoController> Logger;
        private readonly IMapper Mapper;
        public InscripcionPagoController(KalumDbContext _dbContext, ILogger<InscripcionPagoController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionPago>>> Get()
        {
            List<InscripcionPago> resultados = await DbContext.InscripcionPago.Include(ip => ip.Aspirantes).ToListAsync();
            if(resultados == null || resultados.Count == 0)
            {
                return new NoContentResult();
            }
            return Ok(resultados);
        }

        [HttpGet("{id}", Name="GetInscripcionPago")]
        public async Task<ActionResult<InscripcionPago>> GetInscripcionPago ( string id)
        {
            var resultado = await DbContext.InscripcionPago.Include(ip => ip.Aspirantes).FirstOrDefaultAsync(ip => ip.BoletaPago == id);
            if(resultado == null)
            {
                return new NoContentResult();
            }
            return Ok(resultado);
        }
        [HttpPost]
        public async Task<ActionResult<InscripcionPago>> Post([FromBody] InscripcionPagoCreateDTO value)
        {
            InscripcionPago nuevo = Mapper.Map<InscripcionPago>(value);
            nuevo.BoletaPago = Guid.NewGuid().ToString().ToUpper();
            await DbContext.InscripcionPago.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            return new CreatedAtRouteResult("GetInscripcionPago", new{id = nuevo.BoletaPago}, nuevo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] InscripcionPagoUpdateDTO value)
        {
            InscripcionPago inscripcion = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.BoletaPago == id);
            if(inscripcion == null)
            {
                return BadRequest();
            }
            inscripcion.FechaPago = value.FechaPago;
            inscripcion.Monto = value.Monto;
            await DbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<InscripcionPago>> Delete(string id)
        {
            InscripcionPago inscripcion = await DbContext.InscripcionPago.FirstOrDefaultAsync(ip => ip.BoletaPago == id);
            if(inscripcion == null)
            {
                return NotFound();
            }
            DbContext.InscripcionPago.Remove(inscripcion);
            await DbContext.SaveChangesAsync();
            return NoContent();

        }
    }
}