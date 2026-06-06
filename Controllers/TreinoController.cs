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
        var exist = await _repository.GetTreinoPorIdParaEdicao(treino.Id);

        if (exist == null)
            return NotFound("Treino não encontrado");

        // Atualiza os dados do treino
        _mapper.Map(treino, exist);
        exist.EditadoEm = DateTime.UtcNow;

        var idsRecebidos = treino.Exercicios
            .Where(x => x.Id > 0)
            .Select(x => x.Id)
            .ToHashSet();

        // Remove exercícios que estão no banco mas não vieram no payload
        var exerciciosRemover = exist.Exercicios
            .Where(x => !idsRecebidos.Contains(x.Id))
            .ToList();

        foreach (var ex in exerciciosRemover)
        {
            _repository.Delete(ex);
        }

        // Atualiza exercícios existentes
        foreach (var exPayload in treino.Exercicios.Where(x => x.Id > 0))
        {
            var exBanco = exist.Exercicios
                .FirstOrDefault(x => x.Id == exPayload.Id);

            if (exBanco != null)
            {
                _mapper.Map(exPayload, exBanco);
                exBanco.EditadoEm = DateTime.UtcNow;
            }
        }

        // Adiciona exercícios novos (Id = 0)
        foreach (var exNovo in treino.Exercicios.Where(x => x.Id == 0))
        {
            exNovo.TreinoId = exist.Id;
            exNovo.CriadoEm = DateTime.UtcNow;

            exist.Exercicios.Add(exNovo);
        }

        _repository.Update(exist);

        if (await _repository.SaveChangesAsync())
        {
            return Ok("treino atualizado");
        }

        return BadRequest("Problema ao se comunicar com o banco");
    }
    catch (Exception ex)
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
    [HttpPut("finalizaTreino")]
    public async Task<IActionResult> FinalizaTreino(int treinoId, String observacoes)
    {
        try 
        {
            var treino = await _repository.GetTreinoPorId(treinoId);
            treino.Situacao = true;
            treino.Observacoes = observacoes;

            _repository.Update(treino);

            if (await _repository.SaveChangesAsync() == true)
            {
                return Ok("treino finalizado");
            }           
            return BadRequest("Problema ao se comunicar com o banco");
        }
        catch (System.Exception ex)
        {
            return BadRequest("exception 500 : " + ex);
        }
    }
        
}