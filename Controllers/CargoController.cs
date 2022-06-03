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
            public CargoController (KalumDbContext _dbContext)
            {
                this.DbContext = _dbContext;
            }
            [HttpGet]
            public ActionResult<List<Cargo>> Get()
            {
                List<Cargo> cargos = null;
                cargos = DbContext.Cargo.Include(c => c.CuentaXCobrar).ToList();

                if(cargos == null || cargos.Count == 0)
                {
                    return new NoContentResult();
                }
                return Ok(cargos);
            }
        }
    }
