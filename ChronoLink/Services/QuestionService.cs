using ChronoLink.Models;
using ChronoLink.Repositories;

namespace ChronoLink.Services
{
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

        public void MarkAsFavourite(int id)
        {
            var question = _questionResponseRepository.GetById(id);
            if (question != null)
            {
                question.IsFavourite = true;
                _questionResponseRepository.Update(question);
            }
        }

        public void RemoveFavourite(int id)
        {
            var question = _questionResponseRepository.GetById(id);
            if (question != null)
            {
                question.IsFavourite = false;
                _questionResponseRepository.Update(question);
            }
        }
    }
}
