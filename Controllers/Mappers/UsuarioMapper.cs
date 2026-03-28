using api_fit.Dtos;
using api_fit.Dtos.Create;
using api_fit.Models;
using api_fit.Response;
using AutoMapper;

namespace api_fit.Controllers.Mappers
{
    public class UsuarioMapper : Profile
    {
        public UsuarioMapper()
        {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioResponse>().ReverseMap();
            CreateMap<DadosResponse, Dados>().ReverseMap();
            CreateMap<CreateDadosDto, Dados>().ReverseMap();
            CreateMap<Vo2, CreateVo2>().ReverseMap();
        }
    }
}
