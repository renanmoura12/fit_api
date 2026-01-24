using api_fit.Models;

namespace api_fit.Response
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public DadosResponse? Dados { get; set; }
    }
}
