using System;
using System.IO;
using System.Linq;
using Kwis.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kwis.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                /*var integrationConfig = new ConfigurationBuilder()
                  .AddJsonFile("integrationsettings.json")
                  .AddEnvironmentVariables()
                  .Build();

                var connString = integrationConfig.GetConnectionString("db");
                var dbName = $"test_db_{Guid.NewGuid()}";

                //this.DbContextSettings = new MongoDbContextSettings(connString, dbName);
                //this.DbContext = new MongoDbContext(this.DbContextSettings);

                config.AddConfiguration(integrationConfig);
                */
            });

            builder.ConfigureTestServices(services =>
            {
                //services.AddTransient<IQuizService, QuizService>();

                // Build the service provider.
                var sp = services.BuildServiceProvider();
            });
        }
    }
}
