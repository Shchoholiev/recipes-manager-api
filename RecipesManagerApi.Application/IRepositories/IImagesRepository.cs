using MongoDB.Bson;
using RecipesManagerApi.Domain.Entities;

namespace RecipesManagerApi.Application.IRepositories
{
    public interface IImagesRepository : IBaseRepository<Image>
    {
        Task<Image> GetImageAsync(ObjectId id, CancellationToken cancellationToken);

        Task<Image> GetImageAsync(string md5Hash, CancellationToken cancellationToken);

        Task UpdateAsync(Image image, CancellationToken cancellationToken);
    }
}
