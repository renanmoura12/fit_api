namespace api_fit.Dtos
{
    public class FaltasDto
    {
        public int PacienteId { get; set; }
        public DateTime Data { get; set; }
        public bool Justificado { get; set; }
        public byte[]? Imagem { get; set; }
    }
}
