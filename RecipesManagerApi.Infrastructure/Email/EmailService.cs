using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RecipesManagerApi.Infrastructure.Email
{
    public class EmailService
    {
        private readonly SmtpClient smtpClient;

        public EmailService(EmailSettings emailSettings)
        {
            smtpClient = new SmtpClient(emailSettings.Host, emailSettings.Port)
            {
                Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password),
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

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new EmailServiceException("Failed to send email message", ex);
            }
        }
    }
}