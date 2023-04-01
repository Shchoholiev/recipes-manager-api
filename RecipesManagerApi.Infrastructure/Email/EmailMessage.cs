using System.Collections.Generic;
using System.Net.Mail;

namespace RecipesManagerApi.Infrastructure.Email
{
    public class EmailMessage
    {
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public List<string> Recipients { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public List<EmailAttachment> Attachments { get; set; }
    }
}
