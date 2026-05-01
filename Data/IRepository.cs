using api_fit.Models;
using api_fit.Response;

namespace api_fit.Data
{
    public interface IRepository
    {
        //==========================Genericas======================
        void Add<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveChangesAsync();


        Task<bool> LoginAsync(string email, string senha);
        Task<bool> UsuarioExisteAsync(string email);
        Task<UsuarioResponse> GetUsuarioByIdAsync(int id);
        Task<UsuarioResponse> GetUsuarioByEmailAsync(string email);
        Task<IEnumerable<UsuarioResponse>> GetTodosUsuariosAsync();
        Task<(int total, IEnumerable<Usuario> alunoProfessor)> GetAlunoProfessor(int? professorId, int page, int size, string? search, int tipo);

        Task<(int total, IEnumerable<Treino> treinos)> GetTreinosPorMesAluno(int alunoId, int page, int size, string month);
        Task<Treino> GetTreinoPorId(int treinoId);
        Task<Exercicio> GetExercicioPorId(int exercicioId);
        Task<Dados> GetDadosPorId(int id);
        Task<Dados?> GetDadosPorUserId(int userId);
        Task<Treino> GetTreinoPorData(DateTime data);
    }
}
