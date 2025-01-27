using System.Security.Claims;
using ChronoLink.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ChronoLink.Authorization
{
    public class WorkspaceRoleHandler : AuthorizationHandler<WorkspaceRoleRequirement>
    {
        private readonly AppDbContext _context;

        public WorkspaceRoleHandler(AppDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            WorkspaceRoleRequirement requirement
        )
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Fail();
                return;
            }

            // Extract workspaceId from the route or query parameters
            if (context.Resource is HttpContext httpContext)
            {
                var workspaceId = httpContext.Request.RouteValues["workspaceId"] as string;
                if (
                    string.IsNullOrEmpty(workspaceId)
                    || !int.TryParse(workspaceId, out var workspaceIdInt)
                )
                {
                    context.Fail();
                    return;
                }

                // Check if the user has the required role in the workspace
                var workspaceUser = await _context.WorkspaceUsers.FirstOrDefaultAsync(wu =>
                    wu.UserId == userId && wu.WorkspaceId == workspaceIdInt
                );

                if (workspaceUser != null && workspaceUser.Role == requirement.Role)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
        }
    }
}
