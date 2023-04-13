using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Application.Exceptions;

public class DeleteEntityException : Exception
{
    public DeleteEntityException() {}

    public DeleteEntityException(string entityName)
        : base($"\"{entityName}\" can not be deleted.") { }

    public DeleteEntityException(string message, Exception innerException)
        : base(message, innerException) { }
}
