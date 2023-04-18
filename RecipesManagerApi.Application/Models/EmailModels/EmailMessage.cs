namespace RecipesManagerApi.Application.Models.EmailModels
{
    public class EmailMessage
    {
        public List<string> Recipients { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public List<EmailAttachment>? Attachments { get; set; }
    }
}
