using api_fit.Models;

namespace api_fit.Response
{
    public class ListaFormularioResponse
    {
        public Paciente Paciente { get; set; }
        public IEnumerable<FormularioResponse> Formularios { get; set; }
    }
}
