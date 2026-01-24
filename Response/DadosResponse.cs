namespace api_fit.Response;

public class DadosResponse
{
    public int Id { get; set; }
    public DateTime? Nascimento { get; set; }
    public string Genero { get; set; }
    public string Camisa { get; set; }
    public string Cpf { get; set; }
    public string Telefone { get; set; }

    // Address
    public string Rua { get; set; }
    public string Numero { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }

    // Athlete details
    public string NivelCorrida { get; set; }
    public string Objetivo { get; set; }
    public int? DiasTreinoSemana { get; set; }
    public string PaceAtual { get; set; }

    // Plan details
    public string Plano { get; set; }
    public string? Vencimento { get; set; }
    public string? Matricula { get; set; }
    public string? Restricoes { get; set; }

    public DateTime CriadoEm { get; set; }
    public DateTime? EditadoEm { get; set; }
}