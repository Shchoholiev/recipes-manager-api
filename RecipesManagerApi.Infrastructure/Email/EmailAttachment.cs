namespace RecipesManagerApi.Infrastructure.Email
{
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
