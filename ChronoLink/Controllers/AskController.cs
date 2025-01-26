using System.Security.Claims;
using System.Text;
using ChronoLink.Models;
using ChronoLink.Repositories;
using ChronoLink.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChronoLink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AskController : ControllerBase
    {
        private readonly IGeminiService _geminiService;
        private readonly ICalendarRepository _calendarRepository;
        private readonly IQuestionResponseRepository _questionResponseRepository;

        public AskController(
            IGeminiService geminiService,
            ICalendarRepository calendarRepository,
            IQuestionResponseRepository questionResponseRepository
        )
        {
            _geminiService = geminiService;
            _calendarRepository = calendarRepository;
            _questionResponseRepository = questionResponseRepository;
        }

        [HttpPost]
        public IActionResult Ask([FromBody] AskRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var prompt = BuildPrompt(userId, request.WorkspaceId);
                var response = _geminiService.AskGemini(prompt, request.Question);

                var questionResponse = new QuestionResponse
                {
                    Question = request.Question,
                    Response = response,
                    WorkspaceId = request.WorkspaceId,
                    UserId = userId!,
                };

                _questionResponseRepository.Add(questionResponse);

                return Ok(new { Response = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        private string BuildPrompt(string userId, int? workspaceId)
        {
            var calendar = _calendarRepository.GetCalendar(userId, workspaceId);
            if (calendar == null)
                throw new Exception("Calendar not found");

            var prompt = new StringBuilder();
            prompt.AppendLine("Calendar Events:");
            foreach (var evt in calendar.Events)
            {
                prompt.AppendLine(
                    $"{evt.Description} from {evt.StartDateTime} to {evt.EndDateTime}"
                );
            }

            return prompt.ToString();
        }
    }

    public class AskRequest
    {
        public int? WorkspaceId { get; set; } // Null for personal calendar
        public string Question { get; set; } = string.Empty;
    }
}
