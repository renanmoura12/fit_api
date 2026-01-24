using api_fit.Dtos;
using api_fit.Models;
using api_fit.Response;
using AutoMapper;

namespace api_fit.Controllers.Mappers
{
    public class FormularioMapper : Profile
    {
        public FormularioMapper()
        {
            CreateMap<Formulario, ListaFormularioResponse>().ReverseMap();
            CreateMap<Paciente, ListaFormularioResponse>().ReverseMap();
            CreateMap<Formulario, FormularioResponse>().ReverseMap();
            CreateMap<Paciente, Formulario>().ReverseMap();
            CreateMap<Formulario, FormularioDto>().ReverseMap()
                .ForMember(dst => dst.Form, opt => opt.MapFrom(src => src.FormJson.ToString()));
            CreateMap<Formulario, FormPacienteResponse>().ReverseMap();
        }
    }
}
