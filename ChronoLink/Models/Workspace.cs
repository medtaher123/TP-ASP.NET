using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChronoLink.Models
{
    public class Workspace
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Admin who created the workspace
        public string AdminId { get; set; } = string.Empty;
        public User Admin { get; set; }

        // Navigation properties
        public ICollection<WorkspaceUser> WorkspaceUsers { get; set; } = new List<WorkspaceUser>();
        public Calendar SharedCalendar { get; set; }
    }
}
