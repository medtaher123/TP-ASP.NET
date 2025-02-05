using System.Security.Claims;
using ChronoLink.Models;
using ChronoLink.Repositories;
using System.Collections.Generic;

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
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionResponseRepository _questionResponseRepository;

        public QuestionService(IQuestionResponseRepository questionResponseRepository)
        {
            _questionResponseRepository = questionResponseRepository;
        }

        public IEnumerable<QuestionResponse> GetQuestions(string userId)
        {
            return _questionResponseRepository.GetQuestions(userId);
        }

        public void MarkAsFavourite(int id, string userId)
        {
            var userOwnsQuestion = userHasAccess(userId, id);
            if (!userOwnsQuestion)
            {
                throw new UnauthorizedAccessException();
            }
            var question = _questionResponseRepository.GetById(id);
            if (question != null)
            {
                question.IsFavourite = true;
                _questionResponseRepository.Update(question);
            }
        }

        public void RemoveFavourite(int id, string userId)
        {
            var userOwnsQuestion = userHasAccess(userId, id);
            if (!userOwnsQuestion)
            {
                throw new UnauthorizedAccessException();
            }
            var question = _questionResponseRepository.GetById(id);
            if (question != null)
            {
                question.IsFavourite = false;
                _questionResponseRepository.Update(question);
            }
        }
        public IEnumerable<QuestionResponse> GetFavouriteQuestions(string userId)
        {
            return _questionResponseRepository.GetFavouriteQuestions(userId);
        }
        public void RemoveQuestion(int id, string userId)
        {
            var userOwnsQuestion = userHasAccess(userId, id);
            if (!userOwnsQuestion)
            {
                throw new UnauthorizedAccessException();
            }
            _questionResponseRepository.Delete(id);
        }
        public QuestionResponse GetQuestion(int id, string userId)
        {
            var userOwnsQuestion = userHasAccess(userId, id);
            if (!userOwnsQuestion)
            {
                throw new UnauthorizedAccessException();
            }
            return _questionResponseRepository.GetById(id);
        }
        private bool userHasAccess(string userId, int questionId)
        {
            var question = _questionResponseRepository.GetById(questionId);
            if (question == null)
            {
                return false;
            }
            return question.UserId == userId;
        }
    }
}
