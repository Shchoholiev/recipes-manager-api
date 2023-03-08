using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Application.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : EntityBase
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
