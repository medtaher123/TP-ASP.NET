using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChronoLink.Models
{
    public class Calendar
    {
        public int Id { get; set; }

        // Nullable foreign keys to distinguish between personal and shared calendars
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }

        public int? WorkspaceId { get; set; }
        public Workspace Workspace { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
