using System;
using System.Net.Http;
using Kwis.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Kwis.Tests
{
    public abstract class IntegrationTestBase : IClassFixture<DatabaseFixture>
    {
        protected readonly DatabaseFixture fixture;
        protected readonly IConfiguration configuration;

        public IntegrationTestBase(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
    }
}
