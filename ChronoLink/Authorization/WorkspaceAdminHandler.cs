using System.ComponentModel;
using System.Security.Claims;
using System.Text.Json;
using ChronoLink.Data;
using ChronoLink.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ChronoLink.Authorization
{
    public class WorkspaceAdminHandler : AuthorizationHandler<WorkspaceAdminRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public WorkspaceAdminHandler(
            IHttpContextAccessor httpContextAccessor,
            AppDbContext dbContext
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        protected override async System.Threading.Tasks.Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            WorkspaceAdminRequirement requirement
        )
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return;
            }
            // Try to extract workspaceId
            var workspaceId = await TryGetWorkspaceId(httpContext);
            if (!workspaceId.HasValue)
            {
                return;
            }

            // Get the current user ID from the JWT token
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            // Query the database to check if the user is an admin for the workspace
            var isAdmin = await _dbContext.WorkspaceUsers.AnyAsync(wu =>
                wu.UserId == userId
                && wu.WorkspaceId == workspaceId
                && wu.Role == WorkspaceRole.Admin
            );

            if (isAdmin)
            {
                context.Succeed(requirement);
            }
        }

        private async Task<int?> TryGetWorkspaceId(HttpContext httpContext)
        {
            // 1. Check route parameters
            if (
                httpContext.Request.RouteValues.TryGetValue("workspaceId", out var routeValue)
                && int.TryParse(routeValue?.ToString(), out var workspaceIdFromRoute)
            )
            {
                return workspaceIdFromRoute;
            }

            // 2. Check query parameters
            if (
                httpContext.Request.Query.TryGetValue("workspace", out var queryValue)
                && int.TryParse(queryValue, out var workspaceIdFromQuery)
            )
            {
                return workspaceIdFromQuery;
            }

            // 3. Check if taskid is route parameter
            if (
                httpContext.Request.RouteValues.TryGetValue("taskId", out var taskValue)
                && int.TryParse(taskValue?.ToString(), out var taskId)
            )
            {
                var task = await _dbContext.Tasks.FindAsync(taskId);
                if (task != null)
                {
                    var workspaceUser = await _dbContext.WorkspaceUsers.FirstOrDefaultAsync(wu =>
                        wu.Id == task.WorkspaceUserId
                    );
                    return workspaceUser?.WorkspaceId;
                }
            }

            return null;
        }
    }
}
