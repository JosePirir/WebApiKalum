using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WebApiKalum;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;


namespace WebApiKalum.Controllers
{   
    [ApiController]
    [Route("v1/KalumManagement/Inscripciones")]
    public class InscripcionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InscripcionController> Logger;
        private readonly IMapper Mapper;

        public InscripcionController(KalumDbContext _DbContext, ILogger<InscripcionController> _Logger, IMapper _Mapper)
        {
            this.Logger = _Logger;
            this.DbContext = _DbContext;
            this.Mapper = _Mapper;
        }
        [HttpPost("Enrollments")]
        public async Task<ActionResult<ResponseEnrollmentDTO>>EnrollmentCreateAsync([FromBody] EnrollmentDTO value)
        {
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == value.NoExpediente);
            if(aspirante == null)
            {
                //
                return NoContent();

            }
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == value.CarreraId);
            if(carreraTecnica == null)
            {
                return NoContent();
            }
            bool respuesta = await CrearSolicitudAsync(value);
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
            factory.HostName = "LocalHost";
            factory.VirtualHost="/";
            factory.Port = 5672;
            factory.UserName="guest";
            factory.Password="guest";

            try
            {
                conexion = factory.CreateConnection();
                channel = conexion.CreateModel();
                channel.BasicPublish("kalum.exchange.enrollment","",null,Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
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

    }
}