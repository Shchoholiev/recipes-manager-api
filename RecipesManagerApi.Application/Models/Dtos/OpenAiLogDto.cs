namespace RecipesManagerApi.Application.Models.Dtos;

public class OpenAiLogDto
{
    public string Id { get; set; }

    public string Request { get; set; }

    public string Response { get; set; }

    public string CreatedById { get; set; }

    public DateTime CreatedDateUtc { get; set; }
}
