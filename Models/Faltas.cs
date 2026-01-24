namespace api_fit.Models
{
    public class Faltas
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public DateTime Data { get; set; }
        public bool Justificado { get; set; }
        public byte[]? Imagem { get; set; }

        public Paciente Paciente { get; set; }
    }
}
