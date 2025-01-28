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

        [Required]
        public string AdminId { get; set; } = string.Empty;

        [ForeignKey("AdminId")]
        public User Admin { get; set; }

        public ICollection<WorkspaceUser> WorkspaceUsers { get; set; } = new List<WorkspaceUser>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
