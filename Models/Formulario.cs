using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_fit.Models
{
    public class Formulario
    {
        [Key]
        public int Id { get; set; }
        public int EspecialidadeId { get; set; }
        public DateTime DataAplicacao { get; set; }
        public string NomeForm { get; set; }
        public int PacienteId { get; set; }
        public string? Form { get; set; }
        public int UsuarioId { get; set; }
        [NotMapped]
        public object FormJson { get; set; }
        public Especialidade? Especialidade { get; set; }
        public Paciente? Paciente { get; set; }
        public Usuario Usuario { get; set; }
    }
}
