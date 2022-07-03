using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{
        [ApiController]
        [Route("v1/KalumManagement/[controller]")]
        public class CargoController : ControllerBase
        {
            private readonly KalumDbContext DbContext;
            private readonly ILogger<CargoController> Logger;
            public CargoController (KalumDbContext _dbContext, ILogger<CargoController> _Logger)
            {
                this.DbContext = _dbContext;
                this.Logger = _Logger;
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
        }
    }
