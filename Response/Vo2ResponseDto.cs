namespace api_fit.Dtos;

public class Vo2ResponseDto
{
    public int Id { get; set; }
    public float Distancia { get; set; }
    public float Tempo { get; set; }
    public DateTime Data { get; set; }
    public float? Resultado { get; set; }
}