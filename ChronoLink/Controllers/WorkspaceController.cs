using System.Security.Claims;
using ChronoLink.Authorization;
using ChronoLink.Data;
using ChronoLink.Models;
using ChronoLink.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // All endpoints require authentication
    public class WorkspaceController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly WorkspaceService _workspaceService;

        public WorkspaceController(AppDbContext dbContext, WorkspaceService workspaceService)
        {
            _dbContext = dbContext;
            _workspaceService = workspaceService;
        }

        // GET /api/workspace
        [HttpGet]
        public async Task<IActionResult> GetAllWorkspaces()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Get workspaces where the user is a member or admin
            var workspaces = await _dbContext
                .WorkspaceUsers.Where(wu => wu.UserId == userId)
                .Select(wu => new
                {
                    wu.Workspace.Id,
                    wu.Workspace.Name,
                    Members = wu.Workspace.WorkspaceUsers.Select(m => new
                    {
                        m.UserId,
                        m.User.Name,
                        m.IsAdmin,
                    }),
                })
                .ToListAsync();

            return Ok(workspaces);
        }

        // GET /api/workspace/{workspaceId}
        [HttpGet("{workspaceId}")]
        [Authorize(Policy = "WorkspaceMember")]
        public async Task<IActionResult> GetWorkspace(int workspaceId)
        {
            var workspace = await _workspaceService.GetWorkspaceAsync(workspaceId);

            if (workspace == null)
            {
                return NotFound();
            }

            return Ok(workspace);
        }

        // POST /api/workspace
        [HttpPost]
        public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Create the workspace
            var workspace = new Workspace { Name = request.Name };
            _dbContext.Workspaces.Add(workspace);
            await _dbContext.SaveChangesAsync();

            // Add the creator as an admin
            var workspaceUser = new WorkspaceUser
            {
                UserId = userId,
                WorkspaceId = workspace.Id,
                IsAdmin = true,
            };
            _dbContext.WorkspaceUsers.Add(workspaceUser);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetWorkspace),
                new { workspaceId = workspace.Id },
                workspace
            );
        }

        // PUT /api/workspace/{workspaceId}
        [HttpPut("{workspaceId}")]
        [Authorize(Policy = "WorkspaceAdmin")]
        public async Task<IActionResult> UpdateWorkspace(
            int workspaceId,
            [FromBody] UpdateWorkspaceRequest request
        )
        {
            var workspace = await _dbContext.Workspaces.FindAsync(workspaceId);
            if (workspace == null)
            {
                return NotFound();
            }

            workspace.Name = request.Name;
            await _dbContext.SaveChangesAsync();

            return Ok(workspace);
        }

        // DELETE /api/workspace/{workspaceId}
        [HttpDelete("{workspaceId}")]
        [Authorize(Policy = "WorkspaceAdmin")]
        public async Task<IActionResult> DeleteWorkspace(int workspaceId)
        {
            var workspace = await _dbContext.Workspaces.FindAsync(workspaceId);
            if (workspace == null)
            {
                return NotFound();
            }

            _dbContext.Workspaces.Remove(workspace);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // POST /api/workspace/{workspaceId}/users
        [HttpPost("{workspaceId}/users")]
        [Authorize(Policy = "WorkspaceAdmin")]
        public async Task<IActionResult> AddUserToWorkspace(
            int workspaceId,
            [FromBody] AddUserRequest request
        )
        {
            var workspace = await _dbContext.Workspaces.FindAsync(workspaceId);
            if (workspace == null)
            {
                return NotFound("Workspace not found.");
            }

            var user = await _dbContext.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the user is already in the workspace
            var existingWorkspaceUser = await _dbContext.WorkspaceUsers.FirstOrDefaultAsync(wu =>
                wu.WorkspaceId == workspaceId && wu.UserId == request.UserId
            );

            if (existingWorkspaceUser != null)
            {
                return BadRequest("User is already in the workspace.");
            }

            // Add the user to the workspace
            var workspaceUser = new WorkspaceUser
            {
                UserId = request.UserId,
                WorkspaceId = workspaceId,
                IsAdmin = request.IsAdmin,
            };
            _dbContext.WorkspaceUsers.Add(workspaceUser);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // PUT /api/workspace/{workspaceId}/users/{userId}
        [HttpPut("{workspaceId}/users/{userId}")]
        [Authorize(Policy = "WorkspaceAdmin")]
        public async Task<IActionResult> UpdateUserRole(
            int workspaceId,
            string userId,
            [FromBody] UpdateUserRoleRequest request
        )
        {
            var workspaceUser = await _dbContext.WorkspaceUsers.FirstOrDefaultAsync(wu =>
                wu.WorkspaceId == workspaceId && wu.UserId == userId
            );

            if (workspaceUser == null)
            {
                return NotFound("User not found in the workspace.");
            }

            workspaceUser.IsAdmin = request.IsAdmin;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // DELETE /api/workspace/{workspaceId}/users/{userId}
        [HttpDelete("{workspaceId}/users/{userId}")]
        [Authorize(Policy = "WorkspaceAdmin")]
        public async Task<IActionResult> RemoveUserFromWorkspace(int workspaceId, string userId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == userId)
            {
                return BadRequest("You cannot remove yourself.");
            }

            var workspaceUser = await _dbContext.WorkspaceUsers.FirstOrDefaultAsync(wu =>
                wu.WorkspaceId == workspaceId && wu.UserId == userId
            );

            if (workspaceUser == null)
            {
                return NotFound("User not found in the workspace.");
            }

            if (workspaceUser.IsAdmin == true)
            {
                return BadRequest("Cannot remove an admin.");
            }

            _dbContext.WorkspaceUsers.Remove(workspaceUser);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }

    // Request models
    public class CreateWorkspaceRequest
    {
        public string Name { get; set; }
    }

    public class UpdateWorkspaceRequest
    {
        public string Name { get; set; }
    }

    public class AddUserRequest
    {
        public string UserId { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class UpdateUserRoleRequest
    {
        public bool IsAdmin { get; set; }
    }
}
