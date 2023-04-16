using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories
{
    public interface ICategoriesRepository : IBaseRepository<Category>
    {
        Task<Category> GetCategoryAsync(ObjectId id, CancellationToken cancellationToken);
    }
}
