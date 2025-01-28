using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronoLink.Models
{
    public class QuestionResponse
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Question { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Response { get; set; } = string.Empty;

        [Required]
        public bool IsFavourite { get; set; } = false;

        // Optional: A response can exist without a workspace
        public int? WorkspaceId { get; set; }

        [ForeignKey("WorkspaceId")]
        public Workspace? Workspace { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
