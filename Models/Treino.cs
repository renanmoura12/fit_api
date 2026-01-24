using System.ComponentModel.DataAnnotations;

namespace api_fit.Models;

public class Treino
{
    [Key]
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public bool? Situacao { get; set; }
    public string? Observacoes { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? EditadoEm { get; set; }
    public int UserId { get; set; }

    public List<Exercicio> Exercicios { get; set; }
}