using System.ComponentModel.DataAnnotations;

namespace api_fit.Models;

public class Exercicio
{
    [Key]
    public int Id { get; set; }
    public string? Tempo { get; set; }
    public string? Detalhe { get; set; }
    public string? Repeticao { get; set; }
    public string? Distancia { get; set; }
    public string? Pausa { get; set; }
    public int TreinoId { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? EditadoEm { get; set; }
}