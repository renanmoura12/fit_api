using api_fit.Validator;
using System.ComponentModel.DataAnnotations;

namespace api_fit.Models
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string NomeMae { get; set; }

        public Paciente(string nome, DateTime dataNascimento, string nomeMae)
        {
            ValidateDomain(nome, nomeMae, dataNascimento);
        }

        private void ValidateDomain(string nome, string nomeMae, DateTime dataNascimento)
        {
            DomainExceptionValidator.When(nome == null, "O nome é obrigatório");
            DomainExceptionValidator.When(nomeMae == null, "O nome da mãe é obrigatório");
            DomainExceptionValidator.When(nome.Length > 250, "O nome não pode ultrapassar os 250 caracteres");
            DomainExceptionValidator.When(nomeMae.Length > 250, "O nome da mãe não pode ultrapassar os 250 caracteres");
            Nome = nome;
            NomeMae = nomeMae;
            DataNascimento = dataNascimento;
        }
    }
}
