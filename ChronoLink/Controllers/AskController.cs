using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using ChronoLink.Models;
using ChronoLink.Repositories;
using ChronoLink.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChronoLink.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AskController : ControllerBase
    {
        private readonly IGeminiService _geminiService;
        private readonly IQuestionResponseRepository _questionResponseRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;
        private readonly ITaskService _taskService;

        public AskController(
            IGeminiService geminiService,
            IQuestionResponseRepository questionResponseRepository,
            IAuthorizationService authorizationService,
            UserManager<User> userManager,
            ITaskService taskService
        )
        {
            _geminiService = geminiService;
            _questionResponseRepository = questionResponseRepository;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] AskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Extract userId from the authorization token
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { Error = "User is not authenticated." });
                }

                // Fetch the user from the database to ensure they exist
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Unauthorized(new { Error = "User not found." });
                }

                // Check workspace-specific authorization if WorkspaceId is provided
                if (request.WorkspaceId.HasValue)
                {
                    var authorizationResult = await _authorizationService.AuthorizeAsync(
                        User,
                        request.WorkspaceId.Value,
                        "WorkspaceViewer"
                    );

                    if (!authorizationResult.Succeeded)
                    {
                        return Forbid(); // User does not have access to this workspace
                    }
                }

                // Build the prompt and get the response from Gemini
                var prompt = await BuildPrompt(userId, request.WorkspaceId);
                var response = await _geminiService.AskGeminiAsync(prompt, request.Question);

                // Save the question and response
                var questionResponse = new QuestionResponse
                {
                    Question = request.Question,
                    Response = response,
                    WorkspaceId = request.WorkspaceId,
                    UserId = userId,
                };

                _questionResponseRepository.Add(questionResponse);

                return Ok(new AskResponse { Response = response });
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework like Serilog or ILogger)
                Console.Error.WriteLine($"Error in AskController: {ex}");

                return StatusCode(
                    500,
                    new { Error = "An unexpected error occurred. Please try again later." }
                );
            }
        }

        // TOFIX: Implement the above logic to build the prompt
        private async Task<string> BuildPrompt(string userId, int? workspaceId)
        {
           var prompt = "You are an expert at task management and you were asked to analyze some data and provide a response. " +
                 "Do not answer any questions unrelated to the tasks, whenever an irrelevant question is asked, say that you're a " +
                 "bot for task management and that the user can ask anything he wants about the tasks or get suggestions about tasks. " +
                 "You are given the following data: \n";
    var tasks = await _taskService.GetMyTasksAsync(userId, workspaceId);
    foreach (var task in tasks)
    {
        prompt += $"Task: {task.Description}\n";
        prompt += $"Start: {task.StartDateTime}\n";
        prompt += $"End: {task.EndDateTime}\n";
    }
    return prompt;
        }
    }

    public class AskRequest
    {
        public int? WorkspaceId { get; set; } // Null for personal calendar

        [Required(ErrorMessage = "Question is required")]
        [MaxLength(1000, ErrorMessage = "Question cannot exceed 1000 characters")]
        public string Question { get; set; } = string.Empty;
    }

    public class AskResponse
    {
        public string Response { get; set; } = string.Empty;
    }
}
