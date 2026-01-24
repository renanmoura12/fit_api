using api_fit.Models;

namespace api_fit.Response
{
    public class FormPacienteResponse
    {
        public int Id { get; set; }
        public string NomeForm { get; set; }
        public DateTime DataAplicacao { get; set; }
        public string Form { get; set; }
        public Paciente Paciente { get; set; }
        public Especialidade Especialidade { get; set; }
        public int UsuarioId { get; set; }
    }
}
