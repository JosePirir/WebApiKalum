using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class InversionCarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InversionCarreraTecnicaController> Logger;
        private readonly IMapper Mapper;
        public InversionCarreraTecnicaController (KalumDbContext _dbContext, ILogger<InversionCarreraTecnicaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InversionCarreraTecnica>>> Get()
        {
            List<InversionCarreraTecnica> inversion = await DbContext.InversionCarreraTecnica.Include(ict => ict.CarreraTecnica).ToListAsync();
            if(inversion == null || inversion.Count == 0)
            {
                return new NoContentResult();
            }
            return Ok(inversion);
        }
        [HttpGet("{id}", Name="GetInversionCarreraTecnica")]
        public async Task<ActionResult<InversionCarreraTecnica>> GetInversionCarreraTecnica(string id)
        {
            var resultado = await DbContext.InversionCarreraTecnica.Include(ict => ict.CarreraTecnica).FirstOrDefaultAsync(ict => ict.InversionId == id);
            if(resultado == null)
            {
                return new NoContentResult();
            }
            return Ok(resultado);
        }
//InversionCarreraTecnicaListDTO
        [HttpPost]
        public async Task<ActionResult<InversionCarreraTecnica>> Post([FromBody] InversionCarreraTecnicaListDTO value)
        {
            InversionCarreraTecnica nuevo = Mapper.Map<InversionCarreraTecnica>(value);
            nuevo.InversionId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.InversionCarreraTecnica.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            return new CreatedAtRouteResult("GetInversionCarreraTecnica", new{id=nuevo.InversionId}, nuevo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] InversionCarreraTecnica value)
        {
            InversionCarreraTecnica inversion = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(ict => ict.InversionId == id);
            if(inversion == null)
            {
                return BadRequest();
            }
            inversion.MontoInscripcion = value.MontoInscripcion;
            inversion.NumeroPagos = value.NumeroPagos;
            inversion.MontoPago = value.MontoPago;
            DbContext.Entry(inversion).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<InversionCarreraTecnica>> Delete(string id)
        {
            InversionCarreraTecnica inversion = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(ict => ict.InversionId == id);
            if(inversion == null)
            {
                return NotFound();
            }
            DbContext.InversionCarreraTecnica.Remove(inversion);
            await DbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}