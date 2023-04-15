using System;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Application.Exceptions;

public class EntityIsDeletedException<TEntity> : Exception where TEntity : EntityBase
{
    public EntityIsDeletedException()
        : base($"\"{typeof(TEntity).Name}\" was already deleted.") { }

    public EntityIsDeletedException(string message, Exception innerException)
        : base(message, innerException) { }
}


