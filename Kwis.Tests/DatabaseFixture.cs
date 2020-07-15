using System;
using System.Threading.Tasks;
using Kwis.Models;
using Kwis.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Xunit;

namespace Kwis.Tests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public IDatabaseSettings DatabaseSettings { get; private set; }

        public Task InitializeAsync()
        {
            //Db = new SqlConnection("MyConnectionString");

            // ... initialize data in the test database ...

            var configuration = new ConfigurationBuilder()
                  .AddJsonFile("integrationsettings.json")
                  .AddEnvironmentVariables()
                  .Build();

            this.DatabaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

            var client = new MongoClient(this.DatabaseSettings.ConnectionString);
            var database = client.GetDatabase(this.DatabaseSettings.DatabaseName);

            database.CreateCollection(this.DatabaseSettings.CollectionNameQuiz);
            var quizes = database.GetCollection<Quiz>(this.DatabaseSettings.CollectionNameQuiz);

            for (int i = 1; i <= 7; i++)
            {
                quizes.InsertOneAsync(new Quiz()
                {
                    Name = $"Quiz {i}"
                });
            }

            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            // ... clean up test data from the database ...
            var client = new MongoClient(this.DatabaseSettings.ConnectionString);
            client.DropDatabase(this.DatabaseSettings.DatabaseName);
            return Task.CompletedTask;
        }
    }

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
// This class has no code, and is never created. Its purpose is simply
// to be the place to apply [CollectionDefinition] and all the
// ICollectionFixture<> interfaces.
}
}
