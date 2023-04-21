namespace RecipesManagerApi.Application.Models.EmailModels
{
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
