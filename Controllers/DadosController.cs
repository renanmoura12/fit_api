using api_fit.Data;
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
    public async Task<IActionResult> Post([FromBody] Dados dados)
    {
        try
        {
            var exist = await _repository.GetDadosPorUserId(dados.UserId);

            if (exist != null)
                return BadRequest("perfil já existente");

            _repository.Add(dados);

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
}