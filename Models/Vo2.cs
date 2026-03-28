namespace api_fit.Models;

public class Vo2
{
    public int Id { get; set; }
    public int? UsuarioId { get; set; }
    public float Distancia { get; set; }
    public float Tempo { get; set; }
    public DateTime Data { get; set; }
    public float? Resultado { get; set; }

    public Usuario? Usuario { get; set; }
    
}