using System;
using System.Net.Http;
using Kwis.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Kwis.Tests
{
    public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory<Kwis.Startup>>
    {
        protected readonly CustomWebApplicationFactory<Kwis.Startup> factory;
        protected readonly DatabaseFixture fixture;
        protected readonly HttpClient client;
        protected readonly IConfiguration configuration;

        public IntegrationTestBase(CustomWebApplicationFactory<Kwis.Startup> factory, DatabaseFixture fixture)
        {
            this.factory = factory;
            this.fixture = fixture;
            client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }); ;
        }
    }
}
