using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Kwis.Models;
using Kwis.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Xunit;

namespace Kwis.Tests
{
    [Collection("Database collection")]
    public class QuizServiceIntegrationTests : IntegrationTestBase
    {
        private IQuizService service;

        public QuizServiceIntegrationTests(DatabaseFixture fixture) : base(fixture)
        {
            service = new QuizService(fixture.DatabaseSettings);
        }

        //Todo: prevent the alphabetical methods to define what tests ok and what not

        [Fact]
        public async Task Should_get_all()
        {
            //Arrange
            await fixture.ReInitialize();

            //Act
            var quizzes = service.Get();

            //Assert
            quizzes.Result.Should().HaveCount(7);
        }

        [Fact]
        public void Should_throw_for_unknown_Id()
        {
            //Act
            var task = service.Get(Guid.NewGuid());

            //Assert
            task.Result.Should().BeNull();
        }

        [Fact]
        public async Task Should_be_ok_for_known_IdAsync()
        {
            //Arrange
            await fixture.ReInitialize();

            //Act
            Quiz quiz = GetFirstQuizFromTheCurrentDataset();
            Quiz result = null;
            Func<Task> act = async () => result = await service.Get(quiz.Id);

            //Act-Assert
            act.Should().NotThrow();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo<Quiz>(quiz);
        }

        [Fact]
        public async Task Should_delete_and_be_one_lessAsync()
        {
            //Arrange
            await fixture.ReInitialize();
            var originals = await service.Get();
            var takeOne = originals.FirstOrDefault();

            //Act
            bool? result = null;
            Func<Task> act = async () => result = await service.Remove(takeOne);

            // Act-Assert
            act.Should().NotThrow();
            result.Should().BeTrue();

            // Act-Assert
            var control = service.Get();
            control.Result.Should().HaveCount(6);
        }

        [Fact]
        public async Task Should_not_delete_when_delete_unknownAsync()
        {
            //Arrange
            await fixture.ReInitialize();
            var newQuiz = new Quiz
            {
                Name = "Unknown",
                Id = Guid.NewGuid()
            };

            //Act
            var task = service.Remove(newQuiz);

            // Act-Assert
            task.Result.Should().BeFalse();

            // Act-Assert
            var control = service.Get();
            control.Result.Should().HaveCount(7);
        }

        /*
        [Theory]
        [InlineData("/endoint1")]
        [InlineData("/endoint2/details")]
        [InlineData("/endoint3?amount=10&page=1")]
        public async Task Smoketest_Should_ResultInOK(string endpoint)
        {
            var response = await client.GetAsync(endpoint);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }*/

        private Quiz GetFirstQuizFromTheCurrentDataset()
        {
            return fixture.Database.GetCollection<Quiz>(fixture.DatabaseSettings.CollectionNameQuiz).Find(q => true).FirstOrDefault();
        }
    }
}
