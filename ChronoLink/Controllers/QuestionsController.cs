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
            Console.WriteLine(userId);
            var questions = _questionService.GetQuestions(userId!);
            return Ok(questions);
        }
        [HttpGet("{id}")]
        public IActionResult GetQuestion(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var question = _questionService.GetQuestion(id, userId!);
                return Ok(question);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
        [HttpGet("favourite")]
        public IActionResult GetFavouriteQuestions()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var questions = _questionService.GetFavouriteQuestions(userId!);
                return Ok(questions);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }


        [HttpPost("favourite/{id}")]
        public IActionResult MarkAsFavourite(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _questionService.MarkAsFavourite(id, userId!);
                return Ok(new { Message = "Question marked as favourite" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpDelete("favourite/{id}")]
        public IActionResult RemoveFavourite(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _questionService.RemoveFavourite(id, userId!);
                return Ok(new { Message = "Question removed from favourites" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _questionService.RemoveQuestion(id, userId!);
                return Ok(new { Message = "Question removed" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
    }
}
