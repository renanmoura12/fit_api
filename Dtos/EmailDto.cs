using System.ComponentModel.DataAnnotations;

namespace api_fit.Dtos
{
    public class EmailDto
    {
        [EmailAddress(ErrorMessage = "Formato do email inválido")]
        [Required(ErrorMessage = "O Email deve ser informado", AllowEmptyStrings = false)]
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
