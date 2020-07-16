using System;
using System.Net.Http;
using Kwis.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Kwis.Tests.Controller
{
    public abstract class ControllerIntegrationTestBase : IClassFixture<CustomWebApplicationFactory<Kwis.Startup>>
    {
        protected readonly CustomWebApplicationFactory<Kwis.Startup> factory;
        protected HttpClient client;
        protected readonly IConfiguration configuration;

        public ControllerIntegrationTestBase(CustomWebApplicationFactory<Kwis.Startup> factory)
        {
            this.factory = factory;
            client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }); ;
        }
    }
}
