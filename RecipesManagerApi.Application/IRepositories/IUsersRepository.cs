using MongoDB.Bson;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Application.IRepositories;

public interface IUsersRepository : IBaseRepository<User>
{
    Task<List<User>> GetUserPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

    Task<List<User>> GetUserPageAsync(PageParameters pageParameters, Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);

    Task<int> GetTotalCountAsync();

    Task<User> GetUserAsync(ObjectId id, CancellationToken cancellationToken);

    Task UpdateUserAsync(User user, CancellationToken cancellationToken);   
}
