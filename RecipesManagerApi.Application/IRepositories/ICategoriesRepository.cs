using MongoDB.Bson;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories
{
    public interface ICategoriesRepository
    {
        Task<PagedList<Category>> GetPageCategoriesAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task<PagedList<Category>> GetPageCategoriesAsync(PageParameters pageParameters, Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken);

        Task<Category> GetCategoryAsync(ObjectId id, CancellationToken cancellationToken);
    }
}
