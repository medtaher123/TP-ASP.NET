using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChronoLink.Validation;

namespace ChronoLink.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        [EndDateTimeAfterStartDateTime]
        public DateTime EndDateTime { get; set; }

        [Required]
        public int WorkspaceUserId { get; set; }

        [ForeignKey("WorkspaceUserId")]
        public WorkspaceUser WorkspaceUser { get; set; }
    }
}
