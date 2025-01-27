using ChronoLink.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChronoLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkspaceController : ControllerBase
    {
        [HttpPut("{workspaceId}")]
        [Authorize(Policy = "WorkspaceAdmin")]
        public IActionResult UpdateWorkspace(int workspaceId, [FromBody] Workspace workspace)
        {
            // Only users with the "Admin" role in this workspace can access this endpoint
            // Your logic here
            return Ok();
        }

        [HttpGet("{workspaceId}/calendar")]
        [Authorize(Policy = "WorkspaceViewer")]
        public IActionResult GetWorkspaceCalendar(int workspaceId)
        {
            // Only users with the "Viewer" or "Admin" role in this workspace can access this endpoint
            // Your logic here
            return Ok();
        }
    }
}
