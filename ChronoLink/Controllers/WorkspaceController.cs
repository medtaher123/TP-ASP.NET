using System.Security.Claims;
using ChronoLink.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChronoLink.Controllers
{
    [Route("api/workspace/{workspaceId}/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        [HttpPost("{userId}/promote")]
        [Authorize(Policy = "WorkspaceAdmin")]
        public IActionResult PromoteUser(int workspaceId, string userId)
        {
            // Logic to promote user to admin
            return Ok(new { Message = "User promoted to admin" });
        }
    }
}
