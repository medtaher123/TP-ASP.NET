using System.Collections.Generic;
using ChronoLink.Models;

namespace ChronoLink.Services
{
    public interface IQuestionService
    {
        IEnumerable<QuestionResponse> GetQuestions(string userId);
        void MarkAsFavourite(int id);
        void RemoveFavourite(int id);
    }
}
