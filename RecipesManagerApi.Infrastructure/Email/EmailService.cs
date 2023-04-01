using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RecipesManagerApi.Infrastructure.Email
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService(IConfiguration configuration)
        {
            _smtpClient = new SmtpClient(configuration["SmtpHost"], Int32.Parse(configuration["SmtpPort"]))
            {
                Credentials = new NetworkCredential(configuration["SmtpUsername"], configuration["SmtpPassword"]),
                EnableSsl = true
            };
        }

        public async Task SendEmailMessageAsync(EmailMessage emailMessage)
        {
            try
            {
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(emailMessage.Sender, emailMessage.SenderName),
                    Subject = emailMessage.Subject,
                    Body = emailMessage.Body
                };

                foreach (var recipient in emailMessage.Recipients)
                {
                    message.To.Add(recipient);
                }

                if (emailMessage.Attachments != null)
                {
                    foreach (var attachment in emailMessage.Attachments)
                    {
                        message.Attachments.Add(attachment);
                    }
                }

                await _smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new EmailServiceException("Failed to send email message", ex);
            }
        }
    }
}