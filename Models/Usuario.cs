using api_fit.Validator;
using System.ComponentModel.DataAnnotations;

namespace api_fit.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        
        public Dados? Dados { get; set; }

        public Usuario(string nome, string email)
        {
            ValidateDomain(nome, email);
        }

        public Usuario(int id, string nome, string email)
        {
            DomainExceptionValidator.When(id < 0, "O id não pode ser negativo");
            Id = id;
            ValidateDomain(nome, email);
        }

        public void AlterarSenha(byte[] passwordHash, byte[] passwordSalt)
        {
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        private void ValidateDomain(string nome, string email)
        {
            DomainExceptionValidator.When(nome == null, "O nome é obrigatório");
            DomainExceptionValidator.When(email == null, "O email é obrigatório");
            DomainExceptionValidator.When(nome.Length > 250, "O nome não pode ultrapassar os 250 caracteres");
            DomainExceptionValidator.When(email.Length > 200, "O email não pode ultrapassar os 200 caracteres");
            Nome = nome;
            Email = email;
        }
    }
}
