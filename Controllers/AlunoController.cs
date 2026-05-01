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
    
    //quando é aluno pode passar o professorId e tipo 1
    //quando é para buscar todos os professores passa o tipo 2
    [HttpGet]
    public async Task<ActionResult<AlunoResponse>> GetAlunoProfessor(int page, int size, int? professorId, string? search, int tipo)
    {
        try
        {
            var (total, alunos) = await _repository.GetAlunoProfessor(professorId, page, size, search, tipo);

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