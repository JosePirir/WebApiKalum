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
        public class CargoController : ControllerBase
        {
            private readonly KalumDbContext DbContext;
            private readonly ILogger<CargoController> Logger;
            private readonly IMapper Mapper;
            public CargoController (KalumDbContext _dbContext, ILogger<CargoController> _Logger, IMapper _Mapper)
            {
                this.DbContext = _dbContext;
                this.Logger = _Logger;
                this.Mapper = _Mapper;
            }
            
            [HttpGet]
            public async Task<ActionResult<List<Cargo>>> Get()
            {
                List<Cargo> cargos = null;
                Logger.LogDebug("Iniciando proceso en la base de datos");

                cargos = await DbContext.Cargo.Include(c => c.CuentaXCobrar).ToListAsync();

                if(cargos == null || cargos.Count == 0)
                {
                    Logger.LogWarning("No existen cargos");
                    return new NoContentResult();
                }
                Logger.LogInformation("Finalizado el proceso de mostrar Cargos");
                return Ok(cargos);
            }

            [HttpGet("{id}", Name="GetCargo")]
            public async Task<ActionResult<Cargo>> GetCargo(string id)
            {
                Logger.LogDebug("Iniciando proceso en la base de datos id "+ id);
                var cargo = await DbContext.Cargo.Include(c => c.CuentaXCobrar).FirstOrDefaultAsync(c => c.CargoId == id);
                if(cargo == null)
                {
                    Logger.LogWarning("No existen cargos con el id " + id);
                    return new NoContentResult();
                }
                Logger.LogInformation("Finalizado el proceso de busqueda de cargos.");
                return Ok(cargo);
            }

            [HttpPost]
            public async Task<ActionResult<Cargo>> Post([FromBody]CargoCreateDTO value)
            {
                Cargo nuevo = Mapper.Map<Cargo>(value);
                nuevo.CargoId = Guid.NewGuid().ToString().ToUpper();
                await DbContext.Cargo.AddAsync(nuevo);
                await DbContext.SaveChangesAsync();
                return new CreatedAtRouteResult("GetCargo", new{id=nuevo.CargoId}, nuevo);
            }

            [HttpPut("{id}")]
            public async Task<ActionResult> Put(string id, [FromBody] CargoCreateDTO value)
            {
                Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
                if(cargo == null)
                {
                    return BadRequest();
                }
                cargo.Descripcion = value.Descripcion;
                cargo.Prefijo = value.Prefijo;
                cargo.Monto = value.Monto;
                cargo.GeneraMora = value.GeneraMora;
                cargo.PorcentajeMora = value.PorcentajeMora;
                await DbContext.SaveChangesAsync();
                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<ActionResult<Cargo>> Delete(string id)
            {
                Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
                if(cargo == null)
                {
                    return NotFound();
                }
                DbContext.Cargo.Remove(cargo);
                await DbContext.SaveChangesAsync();
                return NoContent();
            }
        }
}
