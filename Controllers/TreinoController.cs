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
public class TreinoController : Controller
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public TreinoController(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Treino treino)
    {
        try
        {
            var exist = await _repository.GetTreinoPorData(treino.Data);

            if (exist != null)
                return BadRequest("treino já existente");

            _repository.Add(treino);

            if (await _repository.SaveChangesAsync() == true)
            {
                return Ok("treino registrado");
            }

            return BadRequest("Problema ao se comunicar com o banco");
        }
        catch (System.Exception ex)
        {
            return BadRequest("exception 500 : " + ex);
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Treino treino)
    {
        try
        {
            var exist = await _repository.GetTreinoPorId(treino.Id);

            var objMapeado = _mapper.Map(treino, exist);
            objMapeado.EditadoEm = DateTime.UtcNow;

            _repository.Update(objMapeado);

            if (await _repository.SaveChangesAsync() == true)
            {
                return Ok("treino atualizado");
            }

            return BadRequest("Problema ao se comunicar com o banco");
        }
        catch (System.Exception ex)
        {
            return BadRequest("exception 500 : " + ex);
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<TreinoResponse>> GetTreino(int page, int size, int alunoId, string month)
    {
        try
        {
            var (total, treinos) = await _repository.GetTreinosPorMesAluno(alunoId, page, size, month);

            if (treinos == null)
            {
                return NotFound();
            }

            TreinoResponse result = new()
            {
                Page = page,
                Size = size,
                Treinos = treinos
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest("exception 500 : " + ex);
        }
    }
}