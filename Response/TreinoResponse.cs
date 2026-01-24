using api_fit.Models;

namespace api_fit.Response;

public class TreinoResponse
{
    public int Page { get; set; }
    public int Size { get; set; }

    public IEnumerable<Treino> Treinos { get; set; }
}