using System.Linq.Expressions;
using MongoDB.Bson;
using RecipesManagerApi.Domain.Common;

namespace RecipesManagerApi.Application.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : EntityBase
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task<List<TEntity>> GetPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<List<TEntity>> GetPageAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        Task<int> GetTotalCountAsync();
    }
}
