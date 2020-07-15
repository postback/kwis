using System;
using Kwis.Services;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Xunit;

namespace Kwis.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public IDatabaseSettings DatabaseSettings { get; private set; }

        public DatabaseFixture()
        {
            //Db = new SqlConnection("MyConnectionString");

            // ... initialize data in the test database ...

            var configuration = new ConfigurationBuilder()
                  .AddJsonFile("integrationsettings.json")
                  .AddEnvironmentVariables()
                  .Build();

            this.DatabaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            this.DatabaseSettings.DatabaseName = this.DatabaseSettings.DatabaseName + "_" + Guid.NewGuid();

            var client = new MongoClient(this.DatabaseSettings.ConnectionString);
            var database = client.GetDatabase(this.DatabaseSettings.DatabaseName);
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
            var client = new MongoClient(this.DatabaseSettings.ConnectionString);
            client.DropDatabase(this.DatabaseSettings.DatabaseName);
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
