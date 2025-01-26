using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChronoLink.Models
{
    public class QuestionResponse
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Question { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Response { get; set; }

        [Required]
        public bool IsFavourite { get; set; } = false;

        public int? WorkspaceId { get; set; }
        public Workspace? Workspace { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
    }
}
