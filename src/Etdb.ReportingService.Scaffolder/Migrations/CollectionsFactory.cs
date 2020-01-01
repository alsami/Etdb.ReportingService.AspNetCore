using System.Linq;
using System.Threading.Tasks;
using Etdb.ReportingService.Domain;
using Etdb.ReportingService.Repositories.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Etdb.ReportingService.Scaffolder.Migrations
{
    internal class CollectionsFactory
    {
        private readonly ReportingServiceDocumentDbContext context;
        
        private static readonly string[] Collections =
        {
            $"{nameof(UserRegistration).ToLower()}s",
            $"{nameof(UserAuthentication).ToLower()}s",
        };

        public CollectionsFactory(ReportingServiceDocumentDbContext context)
            => this.context = context;

        public async Task CreateCollectionsAsync()
        {
            var creationTasks = CollectionsFactory.Collections.Select(async collection =>
            {
                if (await CollectionExistsAsync(collection, this.context.Database)) return;

                await CreateCollection(collection, this.context.Database);
            });

            await Task.WhenAll(creationTasks);
        }

        private static async Task<bool> CollectionExistsAsync(string collectionName, IMongoDatabase database)
        {
            var filter = new BsonDocument("name", collectionName);

            var collections = await database.ListCollectionsAsync(new ListCollectionsOptions
            {
                Filter = filter
            });

            return collections.Any();
        }

        private static Task CreateCollection(string collectionName, IMongoDatabase database,
            CreateCollectionOptions? options = null)
            => database.CreateCollectionAsync(collectionName, options);
    }
}