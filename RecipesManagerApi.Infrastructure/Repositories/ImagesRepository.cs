using MongoDB.Bson;
using MongoDB.Driver;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Infrastructure.Database;
using Image = RecipesManagerApi.Domain.Entities.Image;

namespace RecipesManagerApi.Infrastructure.Repositories
{
    public class ImagesRepository : BaseRepository<Image>,  IImagesRepository
    {
        public ImagesRepository(MongoDbContext db) : base(db, "Images") { }

        public async Task<Image> GetImageAsync(ObjectId id, CancellationToken cancellationToken)
        {
            return await (await this._collection.FindAsync(x => x.Id == id, cancellationToken: cancellationToken))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Image> GetImageAsync(string md5Hash, CancellationToken cancellationToken)
        {
            return await (await this._collection.FindAsync(i => i.Md5Hash == md5Hash, cancellationToken: cancellationToken))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateAsync(Image image, CancellationToken cancellationToken)
        {
            var filter = Builders<Image>.Filter.Eq(i => i.Id, image.Id);
            await this._collection.ReplaceOneAsync(filter, image, new ReplaceOptions(), cancellationToken);
        }
    }
}
