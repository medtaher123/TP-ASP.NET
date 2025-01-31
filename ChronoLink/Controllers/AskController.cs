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
        private readonly WorkspaceService _workspaceService;

        public AskController(
            IGeminiService geminiService,
            IQuestionResponseRepository questionResponseRepository,
            IAuthorizationService authorizationService,
            UserManager<User> userManager,
            ITaskService taskService,
            WorkspaceService workspaceService
        )
        {
            _geminiService = geminiService;
            _questionResponseRepository = questionResponseRepository;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _taskService = taskService;
            _workspaceService = workspaceService;
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
            var user = await _userManager.FindByIdAsync(userId);

            var prompt = "You are an expert at task management chatbot and you were asked to analyze some data and provide a response to the connected user. " +
                  "Do not answer any questions unrelated to the tasks or workspace, whenever an irrelevant question is asked, say that you're a " +
                  "bot for task management and that the user can ask anything he wants about the tasks or get suggestions about tasks. " +
                  "Keep in mind that the user can be connected to a workspace, in which case you should provide information about the workspace and its members. " +
                  "tasks are relative to a worksapce, if no workspace is provided, the tasks are relative to the personal calendar (all users workspaces combined). " +
                  "You are given the following data: \n";
            var tasks = await _taskService.GetMyTasksAsync(userId, workspaceId);
            prompt += $"Now is ${DateTime.Now}\n";
            prompt += $"User name: {user.Name}\n";
            prompt += $"User email: {user.Email}\n";
            if (workspaceId == null)
            {
                prompt += "User is connected to the personal calendar\n";
            }
            else
            {
                var workspace = await _workspaceService.GetByIdAsync(workspaceId.Value);
                prompt += $"User is connected to the workspace: {workspace.Name}\n";
                //TODO: get workspace members
            }

            prompt += $"User has {tasks.Count} tasks\n";
            int i = 1;
            foreach (var task in tasks)
            {
                prompt += $"Task {i}:\n";
                prompt += $"    Task description: {task.Description}\n";
                prompt += $"    Start: {task.StartDateTime}\n";
                prompt += $"    End: {task.EndDateTime}\n";

                i++;
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
