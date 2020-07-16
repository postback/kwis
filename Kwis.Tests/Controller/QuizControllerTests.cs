using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Kwis.Models;
using Kwis.Services;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Kwis.Tests.Controller
{
    [Collection("Database collection")]
    public class QuizControllerTests
    {
        private readonly Mock<IQuizService> mockQuizService = new Mock<IQuizService>();
        private static Guid correctGuid = Guid.NewGuid();
        private Guid wrongGuid = Guid.NewGuid();
        private Quiz quiz = new Quiz { Name = "A", Id = correctGuid };
        protected readonly HttpClient client;

        public QuizControllerTests()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory<Kwis.Startup>(services =>
            {
                // setup the swaps
                services.SwapTransient<IQuizService>(provider => mockQuizService.Object);
            });

            // Create an HttpClient which is setup for the test host
            client = factory.CreateClient();

            var quizesSetup = new List<Quiz>() {
                quiz,
                new Quiz{ Name = "B", Id = Guid.NewGuid() },
                new Quiz{ Name = "C", Id = Guid.NewGuid() },
                new Quiz{ Name = "D", Id = Guid.NewGuid() },
                new Quiz{ Name = "E", Id = Guid.NewGuid() },
                new Quiz{ Name = "F", Id = Guid.NewGuid() }
            };

            mockQuizService.Setup(x => x.Get()).Returns(Task.FromResult<IEnumerable<Quiz>>(quizesSetup));
            mockQuizService.Setup(x => x.Get(It.Is<Guid>(s => s == correctGuid))).Returns(Task.FromResult<Quiz>(quiz));
            mockQuizService.Setup(x => x.Get(It.Is<Guid>(s => s == wrongGuid))).Returns(Task.FromResult<Quiz>(null));
            mockQuizService.Setup(x => x.Remove(quiz)).Returns(Task.FromResult<bool>(true));
        }

        //Todo: prevent the alphabetical methods to define what tests ok and what not

        [Fact]
        public async Task Should_get_all()
        {
            //Arrange
            var response = await client.GetAsync("/api/quiz");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var quizzes = JsonConvert.DeserializeObject<Quiz[]>(
              await response.Content.ReadAsStringAsync()
            );
            quizzes.Should().HaveCount(6);
            mockQuizService.Verify(mock => mock.Get(), Times.Once());
        }

        [Fact]
        public async Task Get_should_throw_404_for_unknown_Id()
        {
            var response = await client.GetAsync("/api/quiz/" + wrongGuid.ToString());
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            mockQuizService.Verify(mock => mock.Get(It.Is<Guid>(s => s == wrongGuid)), Times.Once());
        }

        [Fact]
        public async Task Get_Should_return_result_for_known_Id()
        {
            //Act
            var response = await client.GetAsync("/api/quiz/" + correctGuid.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //Arrange-Act
            var quizInResponse = JsonConvert.DeserializeObject<Quiz>(
               await response.Content.ReadAsStringAsync()
             );

            //Assert
            quizInResponse.Should().BeEquivalentTo(quiz);
            mockQuizService.Verify(mock => mock.Get(It.Is<Guid>(s => s == correctGuid)), Times.Once());
        }

        [Fact]
        public async Task Delete_should_call_remove()
        {
            //Act
            var response = await client.DeleteAsync("/api/quiz/" + correctGuid.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //Arrange-Act
            var result = JsonConvert.DeserializeObject<bool>(
               await response.Content.ReadAsStringAsync()
             );

            //Assert
            result.Should().BeTrue();
            mockQuizService.Verify(mock => mock.Get(It.Is<Guid>(s => s == correctGuid)), Times.Once());
            mockQuizService.Verify(mock => mock.Remove(quiz), Times.Once());
        }

        [Fact]
        public async Task Delete_should_be_false_when_not_found()
        {
            //Act
            var response = await client.DeleteAsync("/api/quiz/" + wrongGuid.ToString());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            mockQuizService.Verify(mock => mock.Get(It.Is<Guid>(s => s == wrongGuid)), Times.Once());
            mockQuizService.VerifyNoOtherCalls();
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
    }
}
