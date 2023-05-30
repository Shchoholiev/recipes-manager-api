using RecipesManagerApi.Domain.Enums;

namespace RecipesManagerApi.Application.Models.Dtos;

public class LogDto
{
    public string Text { get; set; }

    public LogLevels Level { get; set; }
}
