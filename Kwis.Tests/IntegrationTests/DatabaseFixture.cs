using System;
using System.Threading.Tasks;
using Kwis.Models;
using Kwis.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Kwis.Tests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public IDatabaseSettings DatabaseSettings { get; private set; }
        public IMongoDatabase Database { get; private set; }

        public Task InitializeAsync()
        {
            // Initialize data in the test database
            var configuration = new ConfigurationBuilder()
                  .AddJsonFile("integrationsettings.json")
                  .AddEnvironmentVariables()
                  .Build();

            DatabaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

            var client = new MongoClient(DatabaseSettings.ConnectionString);
            Database = client.GetDatabase(DatabaseSettings.DatabaseName);

            // Don't duplicate collection creation
            var filter = new BsonDocument("name", DatabaseSettings.CollectionNameQuiz);
            var collections = Database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
            if (!collections.Result.Any())
            {
                Database.CreateCollection(DatabaseSettings.CollectionNameQuiz);
            }

            //Delete data and recreate data
            _ = Initialize();

            return Task.CompletedTask;
        }

        public async Task ReInitialize()
        {
            await Database.GetCollection<Quiz>(DatabaseSettings.CollectionNameQuiz).DeleteManyAsync(q => true);
            await Initialize();
        }

        private async Task Initialize()
        {
            var quizes = Database.GetCollection<Quiz>(DatabaseSettings.CollectionNameQuiz);

            for (int i = 1; i <= 7; i++)
            {
                await quizes.InsertOneAsync(new Quiz()
                {
                    Name = $"Quiz {i}"
                });
            }
        }

        public Task DisposeAsync()
        {
            // Clean up test data from the database
            var client = new MongoClient(this.DatabaseSettings.ConnectionString);
            client.DropDatabase(this.DatabaseSettings.DatabaseName);
            return Task.CompletedTask;
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {

    }
}