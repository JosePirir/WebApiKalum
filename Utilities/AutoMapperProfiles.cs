using AutoMapper;
using WebApiKalum.Dtos;
using WebApiKalum.Entities;


namespace WebApiKalum.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() /*constructor ctor*/
        {
            CreateMap<CarreraTecnicaCreateDTO, CarreraTecnica>(); /*convertir objetos de tipo CarreraTecnicaDTO a CarreraTecnica*/
            CreateMap<CarreraTecnica, CarreraTecnicaCreateDTO>();
            CreateMap<Jornada, JornadaCreateDTO>();
            CreateMap<ExamenAdmision, ExamenAdmisionCreateDTO>();
            CreateMap<Aspirante, AspiranteListDTO>().ConstructUsing( e => new AspiranteListDTO{NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<Inscripcion, Inscripcion_CarreraTecnicaListDTO>();
            CreateMap<Aspirante, Aspirante_CarreraTecnicaListDTO>().ConstructUsing( e => new Aspirante_CarreraTecnicaListDTO{NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<CarreraTecnica, CarreraTecnicaListDTO>();
            CreateMap<AspirantePutDTO, Aspirante>();
            CreateMap<JornadaCreateDTO, Jornada>();
        }
    }
}