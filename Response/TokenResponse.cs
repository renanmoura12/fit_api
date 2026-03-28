using api_fit.Models;

namespace api_fit.Response
{
    public class TokenResponse
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Token { get; set; }

        public DadosResponse? Dados { get; set; }
        public List<Vo2>? Vo2 { get; set; }
    }
}
