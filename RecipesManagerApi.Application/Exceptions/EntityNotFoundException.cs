using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Application.Exceptions;

public class EntityNotFoundException<TEntity> : Exception where TEntity : EntityBase
{
    public EntityNotFoundException()
        : base($"\"{typeof(TEntity).Name}\" was not found.") { }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
