using System.Collections.Generic;
using ChronoLink.Models;

namespace ChronoLink.Repositories
{
    public interface IQuestionResponseRepository : IRepository<QuestionResponse>
    {
        IEnumerable<QuestionResponse> GetQuestions(string userId);
        IEnumerable<QuestionResponse> GetFavouriteQuestions(string userId);
    }

}
