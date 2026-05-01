using api_fit.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_fit.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfessorController : Controller
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public ProfessorController(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
}