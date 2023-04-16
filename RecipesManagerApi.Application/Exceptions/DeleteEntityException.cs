using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Application.Exceptions;

public class DeleteEntityException<TEntity> : Exception where TEntity : EntityBase
{
    public DeleteEntityException()
        : base($"\"{typeof(TEntity).Name}\" can not be deleted.") { }

    public DeleteEntityException(string message) 
        : base($"\"{typeof(TEntity).Name}\" can not be deleted, because: {message}") { }

    public DeleteEntityException(string message, Exception innerException)
        : base(message, innerException) { }
}
