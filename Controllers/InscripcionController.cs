using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiKalum;
using WebApiKalum.Entities;


namespace WebApiKalum.Controllers
{   
    public class Inscripcion
    {
        
    }
/*    [ApiController]
    [Route("v1/KalumManagement/[controller]")]
    public class InscripcionController
    {
        private readonly KalumDbContext DbContext;
        public InscripcionController(KalumDbContext _dbContext)
        {
            this.DbContext = _dbContext;
        }
        [HttpGet]
        public ActionResult<List<Jornada>> Get()
        {
            List<Inscripcion> inscripciones = null;
            inscripciones = DbContext.Inscripcion.Include(i => i.Jornadas)
        }
    }*/
}