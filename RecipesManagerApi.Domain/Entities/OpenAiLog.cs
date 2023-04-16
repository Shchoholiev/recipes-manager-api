using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Domain.Entities;

public class OpenAiLog : EntityBase
{
    public string Request { get; set; }
    
    public string Response { get; set; }
}
