using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ChronoLink.Validation;

namespace ChronoLink.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        [EndDateTimeAfterStartDateTime]
        public DateTime EndDateTime { get; set; }

        // Foreign key to Calendar
        public int CalendarId { get; set; }
        public Calendar Calendar { get; set; }
    }
}
