using System.Security.Claims;
using ChronoLink.Data;
using ChronoLink.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ChronoLink.Authorization
{
    public class WorkspaceMemberHandler : AuthorizationHandler<WorkspaceMemberRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public WorkspaceMemberHandler(
            IHttpContextAccessor httpContextAccessor,
            AppDbContext dbContext
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        protected override async System.Threading.Tasks.Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            WorkspaceMemberRequirement requirement
        )
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return;
            }

            // Get workspace ID from route or query parameter
            var workspaceId =
                httpContext.Request.RouteValues["workspaceId"]?.ToString()
                ?? httpContext.Request.Query["workspace"].ToString();

            if (
                string.IsNullOrEmpty(workspaceId)
                || !int.TryParse(workspaceId, out var workspaceIdInt)
            )
            {
                return;
            }

            // Get the current user ID from the JWT token
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            // Query the database to check if the user is a member of the workspace
            var isMember = await _dbContext.WorkspaceUsers.AnyAsync(wu =>
                wu.UserId == userId && wu.WorkspaceId == workspaceIdInt
            );

            if (isMember)
            {
                context.Succeed(requirement);
            }
        }
    }
}
