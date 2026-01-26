using api_fit.Data;
using api_fit.Dtos.Create;
using api_fit.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_fit.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DadosController : Controller
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public DadosController(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateDadosDto dados)
    {
        try
        {
            var objMapeado = _mapper.Map<Dados>(dados);
            var exist = await _repository.GetDadosPorUserId(objMapeado.UserId);

            if (exist != null)
                return BadRequest("perfil já existente");

            _repository.Add(objMapeado);

            if (await _repository.SaveChangesAsync() == true)
            {
                return Ok("perfil registrado");
            }

            return BadRequest("Problema ao se comunicar com o banco");
        }
        catch (System.Exception ex)
        {
            return BadRequest("exception 500 : " + ex);
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Dados dados)
    {
        try
        {
            var exist = await _repository.GetDadosPorId(dados.Id);

            var objMapeado = _mapper.Map(dados, exist);
            objMapeado.EditadoEm = DateTime.UtcNow;

            _repository.Update(objMapeado);

            if (await _repository.SaveChangesAsync() == true)
            {
                return Ok("perfil atualizado");
            }

            return BadRequest("Problema ao se comunicar com o banco");
        }
        catch (System.Exception ex)
        {
            return BadRequest("exception 500 : " + ex);
        }
    }
    
    [HttpGet("dadosPorUsuarioId")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<Dados>), 200)]
    public async Task<ActionResult<IEnumerable<Dados>>> GetDadosPorUsuarioId(int userId)
    {
        var usuarios = await _repository.GetDadosPorUserId(userId);

        return Ok(usuarios);
    }
}