namespace api_fit.Dtos
{
    public class FormularioDto
    {
        public int EspecialidadeId { get; set; }
        public DateTime DataAplicacao { get; set; }
        public string NomeForm { get; set; }
        public int PacienteId { get; set; }
        public object FormJson { get; set; }
        public int UsuarioId { get; set; }
    }
}
