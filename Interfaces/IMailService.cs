using api_fit.Dtos;

namespace api_fit.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(EmailDto emailRequest);
    }
}
