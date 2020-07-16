using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kwis.Models;

namespace Kwis.Services
{
    public interface IQuizService
    {
        Task<Quiz> Create(Quiz quiz);
        Task<IEnumerable<Quiz>> Get();
        Task<Quiz> Get(Guid id);
        Task<bool> Remove(Quiz quizToRemove);
        Task<bool> Remove(Guid id);
        Task<bool> Update(Guid id, Quiz quizToUpdate);
    }
}