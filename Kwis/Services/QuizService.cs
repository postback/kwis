using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kwis.Models;
using MongoDB.Driver;

namespace Kwis.Services
{
    public class QuizService : IQuizService
    {
        private readonly IMongoCollection<Quiz> quizes;

        public QuizService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            quizes = database.GetCollection<Quiz>(settings.CollectionNameQuiz);
        }

        public async Task<IEnumerable<Quiz>> Get()
        {
            try
            {
                return await quizes.Find(quiz => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
            

        public async Task<Quiz> Get(string id)
        {
            try
            {
                return await quizes.Find<Quiz>(quiz => quiz.Id == id).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Quiz> Create(Quiz quiz)
        {
            await quizes.InsertOneAsync(quiz);
            return quiz;
        }

        public async Task<bool> Update(string id, Quiz quizToUpdate)
        {
            try
            {
                ReplaceOneResult actionResult = await quizes.ReplaceOneAsync(quiz => quiz.Id == id, quizToUpdate);
                return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> Remove(Quiz quizToRemove)
        {
            return await Remove(quizToRemove.Id);
        }

        public async Task<bool> Remove(string id)
        {
            try
            {
                DeleteResult actionResult = await quizes.DeleteOneAsync(quiz => quiz.Id == id);
                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
