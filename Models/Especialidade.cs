using System.ComponentModel.DataAnnotations;

namespace api_fit.Models
{
    public class Especialidade
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
