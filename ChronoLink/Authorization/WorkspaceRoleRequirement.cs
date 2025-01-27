using Microsoft.AspNetCore.Authorization;

namespace ChronoLink.Authorization
{
    public class WorkspaceRoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }

        public WorkspaceRoleRequirement(string role)
        {
            Role = role;
        }
    }
}
