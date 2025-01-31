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

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET /api/tasks?workspace=xxx
        [HttpGet]
        public async Task<IActionResult> GetAllTasks([FromQuery] int? workspace)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var tasks = await _taskService.GetAllTasksAsync(userId, workspace);
                return Ok(tasks);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // GET /api/my-tasks?workspace=xxx
        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks([FromQuery] int? workspace)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var tasks = await _taskService.GetMyTasksAsync(userId, workspace);
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
            [FromQuery] int? workspace,
            [FromBody] CreateTaskRequest request
        )
        {
            try
            {
                var task = await _taskService.CreateTaskAsync(request, workspace);
                return CreatedAtAction(nameof(GetTask), new { taskId = task.Id }, task);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        // GET /api/tasks/{taskId}
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTask(int taskId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

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
        public string Description { get; set; }
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