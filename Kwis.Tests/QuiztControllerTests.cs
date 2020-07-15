using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Kwis.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Kwis.Tests
{
    [Collection("Database collection")]
    public class QuiztControllerTests : IntegrationTestBase
    {
        public QuiztControllerTests(CustomWebApplicationFactory<Kwis.Startup> factory, DatabaseFixture fixture) : base(factory, fixture)
        {
        }

        [Fact]
        public async Task BasicEndPointTest()
        {
            var response = await client.GetAsync("/api/quiz/all");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var forecast = JsonConvert.DeserializeObject<Quiz[]>(
              await response.Content.ReadAsStringAsync()
            );
            forecast.Should().HaveCount(7);
        }

        [Theory]
        [InlineData("/endoint1")]
        [InlineData("/endoint2/details")]
        [InlineData("/endoint3?amount=10&page=1")]
        public async Task Smoketest_Should_ResultInOK(string endpoint)
        {
            var response = await client.GetAsync(endpoint);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
