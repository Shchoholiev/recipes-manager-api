using MongoDB.Bson;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories
{
    public interface ICategoriesRepository : IBaseRepository<Category>
    {
        Task<List<Category>> GetPageCategoriesAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task<List<Category>> GetPageCategoriesAsync(PageParameters pageParameters, Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken);

        Task<long> GetTotalCounAsync();

        Task<Category> GetCategoryAsync(ObjectId id, CancellationToken cancellationToken);
    }
}
