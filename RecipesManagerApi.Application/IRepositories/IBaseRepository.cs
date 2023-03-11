using MongoDB.Bson;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Application.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : EntityBase
    {
        Task<ObjectId> AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task<int> GetTotalCountAsync();
    }
}
