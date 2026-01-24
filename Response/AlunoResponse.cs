namespace api_fit.Response;

public class AlunoResponse
{
    public int Page { get; set; }
    public int Size { get; set; }

    public List<UsuarioResponse> Alunos { get; set; }
}