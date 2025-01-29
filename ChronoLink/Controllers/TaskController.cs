using System.Security.Claims;
using ChronoLink.Data;
using ChronoLink.Models;
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
        private readonly AppDbContext _dbContext;

        public TasksController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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

            IQueryable<Task> query = _dbContext.Tasks;

            // Filter by workspace if provided
            if (workspace.HasValue)
            {
                // Check if the user is a member of the workspace
                var isMember = await _dbContext.WorkspaceUsers.AnyAsync(wu =>
                    wu.UserId == userId && wu.WorkspaceId == workspace.Value
                );

                if (!isMember)
                {
                    return Forbid();
                }

                query = query.Where(t => t.WorkspaceUser.WorkspaceId == workspace.Value);
            }

            var tasks = await query
                .Select(t => new
                {
                    t.Id,
                    t.Description,
                    t.StartDateTime,
                    t.EndDateTime,
                    t.WorkspaceUser.WorkspaceId,
                    AssignedUserId = t.WorkspaceUser.UserId,
                    AssignedUserName = t.WorkspaceUser.User.Name,
                })
                .ToListAsync();

            return Ok(tasks);
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

            IQueryable<Task> query = _dbContext.Tasks.Where(t => t.WorkspaceUser.UserId == userId);

            // Filter by workspace if provided
            if (workspace.HasValue)
            {
                // Check if the user is a member of the workspace
                var isMember = await _dbContext.WorkspaceUsers.AnyAsync(wu =>
                    wu.UserId == userId && wu.WorkspaceId == workspace.Value
                );

                if (!isMember)
                {
                    return Forbid();
                }

                query = query.Where(t => t.WorkspaceUser.WorkspaceId == workspace.Value);
            }

            var tasks = await query
                .Select(t => new
                {
                    t.Id,
                    t.Description,
                    t.StartDateTime,
                    t.EndDateTime,
                    t.WorkspaceUser.WorkspaceId,
                })
                .ToListAsync();

            return Ok(tasks);
        }

        // POST /api/tasks
        [HttpPost]
        [Authorize(Policy = "WorkspaceAdmin")]
        public async Task<IActionResult> CreateTask(
            [FromQuery] int? workspace,
            [FromBody] CreateTaskRequest request
        )
        {
            var workspaceUser = await _dbContext.WorkspaceUsers.FirstOrDefaultAsync(wu =>
                wu.UserId == request.UserId && wu.WorkspaceId == workspace
            );

            // Check if the user is a member of the workspace
            if (workspaceUser == null)
            {
                return Forbid("The user is not a member of the workspace.");
            }

            // Create the task
            var task = new Task
            {
                Description = request.Description,
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                WorkspaceUserId = workspaceUser.Id,
            };

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { taskId = task.Id }, task);
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

            var task = await _dbContext
                .Tasks.Include(t => t.WorkspaceUser)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                return NotFound();
            }
            // Check if the user is a member of the workspace
            var isMember = await _dbContext.WorkspaceUsers.AnyAsync(wu =>
                wu.UserId == userId && wu.WorkspaceId == task.WorkspaceUserId
            );

            if (!isMember)
            {
                return Forbid();
            }

            var workspaceUser = await _dbContext.WorkspaceUsers.FirstOrDefaultAsync(wu =>
                wu.Id == task.WorkspaceUserId
            );

            var userAssigned = await _dbContext.Users.FirstOrDefaultAsync(u =>
                u.Id == workspaceUser.UserId
            );

            return Ok(
                new
                {
                    task.Id,
                    task.Description,
                    task.StartDateTime,
                    task.EndDateTime,
                    AssignedUserId = userAssigned.Id,
                    AssignedUserName = userAssigned.Name,
                }
            );
        }

        // PUT /api/tasks/{taskId}
        [HttpPut("{taskId}")]
        [Authorize(Policy = "WorkspaceAdmin")]
        public async Task<IActionResult> UpdateTask(
            int taskId,
            [FromBody] UpdateTaskRequest request
        )
        {
            var task = await _dbContext
                .Tasks.Include(t => t.WorkspaceUser)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                return NotFound();
            }

            task.Description = request.Description;
            task.StartDateTime = request.StartDateTime;
            task.EndDateTime = request.EndDateTime;

            await _dbContext.SaveChangesAsync();

            return Ok(task);
        }

        // DELETE /api/tasks/{taskId}
        [HttpDelete("{taskId}")]
        [Authorize(Policy = "WorkspaceAdmin")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var task = await _dbContext.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();

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
