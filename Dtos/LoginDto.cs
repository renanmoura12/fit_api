using System.ComponentModel.DataAnnotations;

namespace api_fit.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "O e-mail é obrigatorio")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha é obrigatoria")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
