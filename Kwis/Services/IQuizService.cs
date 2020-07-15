using System.Collections.Generic;
using System.Threading.Tasks;
using Kwis.Models;

namespace Kwis.Services
{
    public interface IQuizService
    {
        Task<Quiz> Create(Quiz quiz);
        Task<IEnumerable<Quiz>> Get();
        Task<Quiz> Get(string id);
        Task<bool> Remove(Quiz quizToRemove);
        Task<bool> Remove(string id);
        Task<bool> Update(string id, Quiz quizToUpdate);
    }
}