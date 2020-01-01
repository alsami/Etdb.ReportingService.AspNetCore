using System.Threading.Tasks;
using Etdb.ReportingService.Scaffolder.Migrations;

namespace Etdb.ReportingService.Scaffolder
{
    internal class DatabaseScaffolder
    {
        private readonly CollectionsFactory collectionsFactory;
        private readonly SampleDataFactory sampleDataFactory;

        public DatabaseScaffolder(CollectionsFactory collectionsFactory, SampleDataFactory sampleDataFactory)
        {
            this.collectionsFactory = collectionsFactory;
            this.sampleDataFactory = sampleDataFactory;
        }

        public async Task ScaffoldAsync()
        {
            await this.collectionsFactory.CreateCollectionsAsync();
            await this.sampleDataFactory.CreateSampleDataAsync();
        }
    }
}