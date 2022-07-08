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
    public class CuentaXCobrarController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CuentaXCobrarController> Logger;/*logs*/
        private readonly IMapper Mapper;
        public CuentaXCobrarController(KalumDbContext _dbContext, ILogger<CuentaXCobrarController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _dbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaXCobrarListDTO>>> Get()
        {
            List<CuentaXCobrar> cuentas = await DbContext.CuentaXCobrar.Include(cxc => cxc.Alumnos).Include(cxc => cxc.Cargos).ToListAsync();
            if(cuentas == null || cuentas.Count == 0)
            {
                return new NoContentResult();
            }
            List<CuentaXCobrarListDTO> lista = Mapper.Map<List<CuentaXCobrarListDTO>>(cuentas);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(lista);
        }

        [HttpGet("{id}", Name="GetCuentaXCobrar")]
        public async Task<ActionResult<CuentaXCobrar>> GetCuentaXCobrar(string id)
        {
            var cuenta = await DbContext.CuentaXCobrar.Include(cxc => cxc.Alumnos).Include(cxc => cxc.Cargos).FirstOrDefaultAsync(cxc => cxc.Carne == id);
            if(cuenta == null)
            {
                return new NoContentResult();
            }
            return Ok(cuenta);
        }
    }
}