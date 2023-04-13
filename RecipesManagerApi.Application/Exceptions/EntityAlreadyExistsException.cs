using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Application.Exceptions;

public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException() {}

    public EntityAlreadyExistsException(string entityName)
        : base($"\"{entityName}\" already exists.") { }

    public EntityAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException) { }

    public EntityAlreadyExistsException(string entityName, string paramName, string paramValue)
        : base($"\"{entityName}\" with {paramName}: \"{paramValue}\" already exists.") { }
}

