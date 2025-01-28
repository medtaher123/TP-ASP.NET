using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronoLink.Models
{
    public class WorkspaceUser
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int WorkspaceId { get; set; }

        [ForeignKey("WorkspaceId")]
        public Workspace Workspace { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public WorkspaceRole Role { get; set; } = WorkspaceRole.Member;
    }

    public enum WorkspaceRole
    {
        Admin,
        Member,
    }
}
