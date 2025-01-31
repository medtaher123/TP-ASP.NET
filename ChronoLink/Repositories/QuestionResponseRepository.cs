using System.Collections.Generic;
using System.Linq;
using ChronoLink.Data;
using ChronoLink.Models;

namespace ChronoLink.Repositories
{
    public class QuestionResponseRepository
        : Repository<QuestionResponse>,
            IQuestionResponseRepository
    {
        private readonly AppDbContext _context;

        public QuestionResponseRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<QuestionResponse> GetQuestions(string userId)
        {
            return _context.QuestionResponses.Where(q => q.UserId == userId).ToList();
        }
        public IEnumerable<QuestionResponse> GetFavouriteQuestions(string userId)
        {
            return _context.QuestionResponses.Where(q => q.UserId == userId && q.IsFavourite).ToList();
        }
        public QuestionResponse GetById(int id)
        {
            return _context.QuestionResponses.Find(id);
        }
        public void Delete(int id)
        {
            var question = GetById(id);
            if (question != null)
            {
                _context.QuestionResponses.Remove(question);
                _context.SaveChanges();
            }
        }
    }
}
