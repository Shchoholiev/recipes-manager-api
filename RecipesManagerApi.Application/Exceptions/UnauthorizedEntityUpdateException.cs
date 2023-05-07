using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Application.Exceptions;

public class UnauthorizedEntityUpdateException<TEntity> : Exception where TEntity : EntityBase
{
    public UnauthorizedEntityUpdateException()
        : base($"\"{typeof(TEntity).Name} cannot be updated as you are not an author") { }

    public UnauthorizedEntityUpdateException(string message, Exception innerException)
        : base(message, innerException) { }
}

