using api_fit.Dtos;
using api_fit.Interfaces;
using api_fit.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace api_fit.Services
{
    public class MailService : IMailService
    {
            private readonly MailSettings _mailSettings;
            public MailService(IOptions<MailSettings> mailSettings)
            {
                _mailSettings = mailSettings.Value;
            }
            public async Task SendEmailAsync(EmailDto mailRequest)
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_mailSettings.Mail);
                    mailMessage.To.Add(new MailAddress(mailRequest.ToEmail));
                    mailMessage.Subject = mailRequest.Subject;
                    mailMessage.Body = mailRequest.Body;
                    mailMessage.IsBodyHtml = true;

                    if (mailRequest.Attachments != null)
                    {
                        foreach (var file in mailRequest.Attachments)
                        {
                            if (file.Length > 0)
                            {
                                var attachment = new Attachment(file.OpenReadStream(), file.FileName);
                                mailMessage.Attachments.Add(attachment);
                            }
                        }
                    }

                    using var smtp = new SmtpClient();
                    smtp.Host = _mailSettings.Host;
                    smtp.Port = _mailSettings.Port;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);

                    await smtp.SendMailAsync(mailMessage);
                }
            }
        }
    }
