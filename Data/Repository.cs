using api_fit.Models;
using api_fit.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace api_fit.Data
{
    public class Repository : IRepository
    {
        public DataContext Context { get; }
        private readonly IMapper _mapper;

        public Repository(DataContext context, IMapper mapper)
        {
            this.Context = context;
            _mapper = mapper;
        }

        //==========================Genericas======================
        public void Add<T>(T entity) where T : class
        {
            Context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            Context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            Context.Update(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await Context.SaveChangesAsync() > 0);
        }

        public async Task<UsuarioResponse> GetUsuarioByIdAsync(int id)
        {
            var usuario = await Context
                .Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            return _mapper.Map<UsuarioResponse>(usuario);
        }

        public async Task<IEnumerable<UsuarioResponse>> GetTodosUsuariosAsync()
        {
            var usuario = await Context.Usuarios.ToListAsync();

            return _mapper.Map<IEnumerable<UsuarioResponse>>(usuario);
        }



        public async Task<bool> LoginAsync(string email, string senha)
        {
            var usuario = await Context.Usuarios.Where(a => a.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return false;
            }

            using var hmac = new HMACSHA512(usuario.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));

            for (int x = 0; x < computedHash.Length; x++)
            {
                if (computedHash[x] != usuario.PasswordHash[x])
                    return false;
            }

            return true;

        }

        public async Task<bool> UsuarioExisteAsync(string email)
        {
            var usuario = await Context.Usuarios.Where(a => a.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

            if (usuario == null)
            {
                return false;
            }

            return true;
        }

        public async Task<UsuarioResponse> GetUsuarioByEmailAsync(string email)
        {
            var usuario = await Context.Usuarios
                .Include(a => a.Dados)
                .AsNoTracking()
                .Where(a => a.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();

            var objMapeado = _mapper.Map<UsuarioResponse>(usuario);

            return objMapeado;

        }

        //tipo 1 aluno, tipo 2 professor
        public async Task<(int total, IEnumerable<Usuario> alunos)> GetAlunos(int? professorId, int page, int size, string? search)
        {
            var query = Context.Usuarios
                .AsNoTracking()
                .Include(a => a.Dados)
                .Where(a => a.Dados.TipoUsuario.Equals(1))
                .AsQueryable();

            if (professorId != null)
                query = query.Where(a => a.Dados.ProfessorId.Equals(professorId));

            if (search != null)
                query = query.Where(a => a.Nome.Equals(search));

            var total = await query.CountAsync();

            var result = await query.Skip((page - 1) * size).Take(size).ToListAsync();

            return (total, result);
        }

        public async Task<(int total, IEnumerable<Treino> treinos)> GetTreinosPorMesAluno(int alunoId, int page,
            int size, string month)
        {
            var query = Context.Treino
                .AsNoTracking()
                .Include(a => a.Exercicios)
                .Where(a => a.UserId.Equals(alunoId) &&
                            a.Data.Month.ToString().Equals(month));

            var total = await query.CountAsync();

            var result = await query.Skip((page - 1) * size).Take(size).ToListAsync();

            return (total, result);
        }

        public async Task<Dados> GetDadosPorId(int id)
        {
            return await Context.Dados.AsNoTracking().Where(a => a.Id == id).FirstOrDefaultAsync();
        }
        
        public async Task<Dados?> GetDadosPorUserId(int userId)
        {
            return await Context.Dados.AsNoTracking().Where(a => a.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Treino> GetTreinoPorId(int treinoId)
        {
            return await Context.Treino.AsNoTracking().Where(a => a.Id == treinoId).FirstOrDefaultAsync();
        }
        
        public async Task<Treino> GetTreinoPorData(DateTime data)
        {
            return await Context.Treino.AsNoTracking().Where(a => a.Data == data).FirstOrDefaultAsync();
        }

        public async Task<Exercicio> GetExercicioPorId(int exercicioId)
        {
            return await Context.Exercicio.AsNoTracking().Where(a => a.Id == exercicioId).FirstOrDefaultAsync();
        }
    }
}
