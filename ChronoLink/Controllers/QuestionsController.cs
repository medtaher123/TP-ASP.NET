using System.Security.Claims;
using ChronoLink.Models;
using ChronoLink.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChronoLink.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public IActionResult GetQuestions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var questions = _questionService.GetQuestions(userId!);
            return Ok(questions);
        }
        [HttpGet("{id}")]
        public IActionResult GetQuestion(int id)
        {
            var question = _questionService.GetQuestion(id);
            return Ok(question);
        }
        [HttpGet("favourite")]
        public IActionResult GetFavouriteQuestions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var questions = _questionService.GetFavouriteQuestions(userId!);
            return Ok(questions);
        }


        [HttpPost("favourite/{id}")]
        public IActionResult MarkAsFavourite(int id)
        {
            _questionService.MarkAsFavourite(id);
            return Ok(new { Message = "Question marked as favourite" });
        }

        [HttpDelete("favourite/{id}")]
        public IActionResult RemoveFavourite(int id)
        {
            _questionService.RemoveFavourite(id);
            return Ok(new { Message = "Question removed from favourites" });
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            _questionService.RemoveQuestion(id);
            return Ok(new { Message = "Question removed" });
        }
    }
}
