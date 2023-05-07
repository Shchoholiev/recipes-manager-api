using RecipesManagerApi.Domain.Common;
using RecipesManagerApi.Domain.Enums;

namespace RecipesManagerApi.Domain.Entities;
public class Log : EntityBase
{
    public string Text { get; set; }

    public LogLevels Level { get; set; }
}
