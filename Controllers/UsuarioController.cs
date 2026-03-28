using api_fit.Data;
using api_fit.Dtos;
using api_fit.Interfaces;
using api_fit.Models;
using api_fit.Response;
using api_fit.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api_fit.Dtos.Create;

namespace api_fit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public UsuarioController(IRepository repository, IMapper mapper, IConfiguration configuration, IMailService mailService)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
            _mailService = mailService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        public async Task<ActionResult<TokenResponse>> IncluiNovoUsuario(UsuarioDTO usuariodto)
        {
            var emailExiste = await _repository.UsuarioExisteAsync(usuariodto.Email);

            if (emailExiste)
            {
                return BadRequest("email ja existe");
            }

            var objmapeado = _mapper.Map<Usuario>(usuariodto);
            using var hmac = new HMACSHA512();
            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("XXXXXXXXS"));
            byte[] passwordSalt = hmac.Key;

            objmapeado.AlterarSenha(passwordHash, passwordSalt);

            _repository.Add(objmapeado);

            await _repository.SaveChangesAsync();

            var token = TokenService.GenerateToken(_configuration, usuariodto.Nome, usuariodto.Email);

            string link = "https://ipesq.netlify.app/definir-senha?token=" + token;

            EmailDto email = new()
            {
                ToEmail = usuariodto.Email,
                Subject = "Cadastre sua senha",
                Body = $@"<html><body>Clique no link para definir sua senha:<br/><br/><a href=""{link}"">{link}</a></body></html>"
            };

            await _mailService.SendEmailAsync(email);

            TokenResponse userToken = new()
            {
                UsuarioId = objmapeado.Id,
                Nome = objmapeado.Nome,
                Token = token
            };

            return userToken;
        }


        [HttpPatch("definirSenha")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Patch(string token, string novaSenha)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["jwt:key"]);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

            var usuario = principal?.FindFirst(ClaimTypes.Email).Value;

            if (usuario != null)
            {
                var result = await _repository.GetUsuarioByEmailAsync(usuario);

                if (result != null)
                {
                    using var hmac = new HMACSHA512();
                    byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(novaSenha));
                    byte[] passwordSalt = hmac.Key;

                    var objMapeado = _mapper.Map<Usuario>(result);

                    objMapeado.AlterarSenha(passwordHash, passwordSalt);

                    _repository.Update(objMapeado);

                    if (await _repository.SaveChangesAsync())
                    {
                        return Ok("Senha atualizada");
                    }
                }
            }

            return BadRequest();
        }

        [HttpPost("redifinirSenha")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Post(string email)
        {
            var result = await _repository.GetUsuarioByEmailAsync(email);

            if (result == null)
                return NotFound("Email não localizado");

            var token = TokenService.GenerateToken(_configuration ,result.Nome, result.Email);

            string link = "https://ipesq.netlify.app/definir-senha?token=" + token;

            EmailDto corpoEmail = new()
            {
                ToEmail = result.Email,
                Subject = "Redefina sua senha",
                Body = $@"<html><body>Clique no link abaixo para redefinir sua senha:<br/><br/><a href=""{link}"">{link}</a></body></html>"
            };

            await _mailService.SendEmailAsync(corpoEmail);

            return Ok("Email enviado com sucesso");
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        public async Task<ActionResult<TokenResponse>> Login(LoginDto login)
        {
            var existe = await _repository.UsuarioExisteAsync(login.Email);
            if (!existe)
            {
                return BadRequest("Usuario não cadastrado");
            }

            var result = await _repository.LoginAsync(login.Email, login.Senha);

            if (!result)
            {
                return Unauthorized("Usuário ou senha inválidos");
            }

            var usuario = await _repository.GetUsuarioByEmailAsync(login.Email);

            var token = TokenService.GenerateToken(_configuration, usuario.Nome, usuario.Email);

            return new TokenResponse()
            {
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Token = token,
                Dados = usuario?.Dados,
                Vo2 = usuario.Vo2
            };
        }

        [HttpGet("todosUsuarios")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<UsuarioResponse>), 200)]
        public async Task<ActionResult<IEnumerable<UsuarioResponse>>> GetListaUsuarios()
        {
            var usuarios = await _repository.GetTodosUsuariosAsync();

            return Ok(usuarios);
        }


        [HttpPost("CreateVo2")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        public async Task<ActionResult<TokenResponse>> CreateVo2(CreateVo2 vo2Dto)
        {
            var velocidade = vo2Dto.Distancia / vo2Dto.Tempo; // m/min
            var vo2Calculado = (velocidade * 0.2f) + 3.5f;

            var mapperObject = _mapper.Map<Vo2>(vo2Dto);
            mapperObject.Resultado = vo2Calculado;

            _repository.Add(mapperObject);
            await _repository.SaveChangesAsync();

            return Ok(mapperObject);
        }
    }
}
