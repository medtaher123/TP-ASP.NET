using System.Security.Claims;
using ChronoLink.Data;
using ChronoLink.Models;
using ChronoLink.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = ChronoLink.Models.Task;

namespace ChronoLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly WorkspaceService _workspaceService;

        public TasksController(ITaskService taskService, WorkspaceService workspaceService)
        {
            _taskService = taskService;
            _workspaceService = workspaceService;
        }

        // GET /api/tasks?workspace=xxx
        [HttpGet]
        public async Task<IActionResult> GetAllTasks([FromQuery] int? workspaceId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var tasks = await _taskService.GetAllTasksAsync(userId, workspaceId);
                return Ok(tasks);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // GET /api/my-tasks?workspace=xxx
        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks([FromQuery] int? workspaceId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var tasks = await _taskService.GetMyTasksAsync(userId, workspaceId);
                return Ok(tasks);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // POST /api/tasks
        [HttpPost]
        [Authorize(Policy = "WorkspaceAdmin")]
        public async Task<IActionResult> CreateTask(
            [FromQuery] int workspaceId,
            [FromBody] CreateTaskRequest request
        )
        {
            try
            {
                var task = await _taskService.CreateTaskAsync(request, workspaceId);
                return CreatedAtAction(nameof(GetTask), new { taskId = task.Id }, task);
            }
            catch (UnauthorizedAccessException ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "Forbidden",
                    Detail = ex.Message,
                    Status = StatusCodes.Status403Forbidden
                };
                return StatusCode(StatusCodes.Status403Forbidden, problemDetails);
            }
        }

        // GET /api/tasks/{taskId}
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTask(int taskId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var task = await _taskService.GetTaskAsync(taskId, userId);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT /api/tasks/{taskId}
        [HttpPut("{taskId}")]
        [Authorize(Policy = "WorkspaceAdmin")]
        public async Task<IActionResult> UpdateTask(
            int taskId,
            [FromBody] UpdateTaskRequest request
        )
        {
            var task = await _taskService.UpdateTaskAsync(taskId, request);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // DELETE /api/tasks/{taskId}]
        [HttpDelete("{taskId}")]
        [Authorize(Policy = "WorkspaceAdmin")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var success = await _taskService.DeleteTaskAsync(taskId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

    // Request models
    public class CreateTaskRequest
    {
        public required string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public required string UserId { get; set; }
    }

    public class UpdateTaskRequest
    {
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}