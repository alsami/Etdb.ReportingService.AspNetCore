using System;
using Etdb.ReportingService.Repositories.Conventions;
using Etdb.ServiceBase.DocumentRepository;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Etdb.ReportingService.Repositories.Models
{
    public class ReportingServiceDocumentDbContext : DocumentDbContext
    {
        public ReportingServiceDocumentDbContext(Func<IMongoDatabase> databaseComposer) : base(databaseComposer)
            => this.Configure();

        public sealed override void Configure()
        {
            MongoDbConventions.UseImmutableConvention();
            MongoDbConventions.UseCamelCaseConvention();
            MongoDbConventions.UseIgnoreNullValuesConvention();
            MongoDbConventions.UseEnumStringRepresentation();
            ConventionRegistry.Register(nameof(GuidIdConvention), new ConventionPack
            {
                new GuidIdConvention()
            }, _ => true);
        }
    }
}