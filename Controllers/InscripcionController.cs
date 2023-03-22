using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WebApiKalum;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;
using WebApiKalum.Services;

namespace WebApiKalum.Controllers
{   
    [ApiController]
    [Route("v1/KalumManagement/Inscripciones")]
    public class InscripcionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InscripcionController> Logger;
        private readonly IMapper Mapper;
        public IUtilsService UtilsService{get;}
        public IConfiguration Configuration {get;}

        public InscripcionController(KalumDbContext _DbContext, ILogger<InscripcionController> _Logger, IMapper _Mapper, IUtilsService _UtilsService)
        {
            this.Logger = _Logger;
            this.DbContext = _DbContext;
            this.Mapper = _Mapper;
            this.UtilsService = _UtilsService;
        }
        [HttpPost("Enrollments")]
        public async Task<ActionResult<ResponseEnrollmentDTO>>EnrollmentCreateAsync([FromBody] EnrollmentDTO value)
        {
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == value.NoExpediente);
            if(aspirante == null)
            {
                return NoContent();
            }
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if(carreraTecnica == null)
            {
                return NoContent();
            }
            bool respuesta = await this.UtilsService.CrearSolicitudAsync(value);   
            if(respuesta == true)
            {
                ResponseEnrollmentDTO response = new ResponseEnrollmentDTO();
                response.HttpStatus = 201;
                response.Message = "El proceso de inscripcion se ha realizado con exito";
                return Ok(response);
            }
            else
            {
                return StatusCode(503, value);
            }
        }

        private async Task<bool> CrearSolicitudAsync(EnrollmentDTO value)
        {
            bool proceso = false;
            ConnectionFactory factory = new ConnectionFactory();
            IConnection conexion = null;
            IModel channel = null;
            factory.HostName = this.Configuration.GetValue<string>("RabbitConfiguration:HostName");;
            factory.VirtualHost = this.Configuration.GetValue<string>("RabbitConfiguration:VirtualHost");;
            factory.Port = this.Configuration.GetValue<int>("RabbitConfiguration:Port");
            factory.UserName = this.Configuration.GetValue<string>("RabbitConfiguration:UserName");
            factory.Password = this.Configuration.GetValue<string>("RabbitConfiguration:Password");

            try
            {
                conexion = factory.CreateConnection();
                channel = conexion.CreateModel();
                channel.BasicPublish("RabbitConfiguration:EnrollmentExchange","",null,Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
                await Task.Delay(100);
                proceso = true;
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                channel.Close();    
                conexion.Close();
            }

            return proceso;
        }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InscripcionListDTO>>> Get()
    {
        List<Inscripcion> inscripcion = await DbContext.Inscripcion.Include(i => i.CarreraTecnica).Include(i=>i.Jornada).Include(i=>i.Alumnos).ToListAsync();
        if(inscripcion == null || inscripcion.Count == 0)
        {
            return new NoContentResult();
        }
        List<InscripcionListDTO> resumen = Mapper.Map<List<InscripcionListDTO>>(inscripcion);
        return Ok(resumen);
    }

    [HttpGet ("{id}", Name="GetInscripcion")]
    public async Task<ActionResult<Inscripcion>> GetInscripcion(string id)
    {
        var inscripcion = await DbContext.Inscripcion.Include(i => i.CarreraTecnica).Include(i => i.Jornada).Include(i => i.Alumnos).FirstOrDefaultAsync(i => i.InscripcionId == id);
        if(inscripcion == null)
        {
            return new NoContentResult();
        }
        return Ok(inscripcion);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(string id, [FromBody] Inscripcion value)
    {
        Inscripcion inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(i => i.InscripcionId == id);
        if(inscripcion == null)
        {
            return BadRequest();
        }
        inscripcion.Ciclo = value.Ciclo;
        inscripcion.FechaInscripcion = value.FechaInscripcion;
        DbContext.Entry(inscripcion).State = EntityState.Modified;
        await DbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Inscripcion>> Delete(string id)
    {
        Inscripcion inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(i => i.InscripcionId == id);
        if(inscripcion == null)
        {
            return NotFound();
        }
        DbContext.Inscripcion.Remove(inscripcion);
        await DbContext.SaveChangesAsync();
        return NoContent();
    }
    }
}