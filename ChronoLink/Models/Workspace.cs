using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChronoLink.Models
{
    public class Workspace
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<WorkspaceUser> WorkspaceUsers { get; set; } = new List<WorkspaceUser>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
