using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;

namespace WebApiKalum.Controllers
{ 
    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        public JornadaController(KalumDbContext _dbContext)
        {
            this.DbContext = _dbContext;
        }
        [HttpGet]
        public ActionResult<List<Jornada>> Get()
        {
            List<Jornada> jornadas = null;
            jornadas = DbContext.Jornada.Include(j => j.Aspirantes).ToList();

            if(jornadas == null || jornadas.Count == 0)
            {
                return new NoContentResult();
            }
            return Ok(jornadas);
        }
    }  
}