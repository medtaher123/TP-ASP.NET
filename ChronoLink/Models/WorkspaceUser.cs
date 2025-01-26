using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChronoLink.Models
{
    public class WorkspaceUser
    {
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }

        public int WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }

        [Required]
        public bool IsAdmin { get; set; }
    }
}
