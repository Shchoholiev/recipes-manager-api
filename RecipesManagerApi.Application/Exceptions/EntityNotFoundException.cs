using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Application.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException() {}

    public EntityNotFoundException(string entityName)
        : base($"\"{entityName}\" was not found.") { }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
