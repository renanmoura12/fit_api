using api_fit.Data;
using api_fit.Models;
using api_fit.Response;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_fit.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlunoController : Controller
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public AlunoController(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<AlunoResponse>> GetAlunoPorProfessorId(int page, int size, int? professorId, string? search)
    {
        try
        {
            var (total, alunos) = await _repository.GetAlunos(professorId, page, size, search);

            if (alunos == null)
            {
                return NotFound();
            }

            AlunoResponse result = new()
            {
                Page = page,
                Size = size,
                Alunos = _mapper.Map<List<UsuarioResponse>>(alunos)
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest("exception 500 : " + ex);
        }
    }
}