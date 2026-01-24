using api_fit.Dtos;
using api_fit.Models;
using api_fit.Response;
using AutoMapper;

namespace api_fit.Controllers.Mappers
{
    public class FaltasMapper : Profile
    {
        public FaltasMapper()
        {
            CreateMap<Faltas, FaltaResponse>().ReverseMap();
            CreateMap<Faltas, FaltasDto>().ReverseMap();
        }
    }
}
