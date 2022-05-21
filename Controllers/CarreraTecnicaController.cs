using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;

namespace WebApiKalumn.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class CarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        public CarreraTecnicaController(KalumDbContext _dbContext)
        {
            this.DbContext = _dbContext;
        }
        [HttpGet]
        public ActionResult<List<CarreraTecnica>> Get()
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            carrerasTecnicas = DbContext.CarreraTecnica.Include(c=>c.Aspirantes).ToList();
            
            if(carrerasTecnicas == null || carrerasTecnicas.Count == 0)
            {
                return new NoContentResult();
            }
            return Ok(carrerasTecnicas);
        }
    }
}