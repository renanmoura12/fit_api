using api_fit.Dtos;
using api_fit.Models;
using AutoMapper;

namespace api_fit.Controllers.Mappers
{
    public class EspecialidadeMapper : Profile
    {
        public EspecialidadeMapper()
        {
            CreateMap<Especialidade, EspecialidadeDto>().ReverseMap()
                .ForMember(dst => dst.Nome, opt => opt.MapFrom(src => src.NomeEspecialidade));
        }
    }
}
