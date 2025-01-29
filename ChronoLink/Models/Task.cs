using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
        [DateTime]
        [StartDateTimeBeforeEndDateTime]
        public DateTime StartDateTime { get; set; }

        [Required]
        [DateTime]
        [EndDateTimeAfterStartDateTime]
        public DateTime EndDateTime { get; set; }

        public int WorkspaceUserId { get; set; }

        [ForeignKey("WorkspaceUserId")]
        [JsonIgnore]
        public WorkspaceUser WorkspaceUser { get; set; }
    }
}
