using System.Collections.Generic;
using ChronoLink.Models;

namespace ChronoLink.Services
{
    public interface IQuestionService
    {
        IEnumerable<QuestionResponse> GetQuestions(string userId);
        void MarkAsFavourite(int id, string userId);
        void RemoveFavourite(int id, string userId);
        IEnumerable<QuestionResponse> GetFavouriteQuestions(string userId);
        void RemoveQuestion(int id, string userId);
        QuestionResponse GetQuestion(int id, string userId);
    }
}
