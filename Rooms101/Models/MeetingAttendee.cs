using Rooms101.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Rooms101.Models
{
    public class MeetingAttendee
    {
        [Key]
        public int MeetingAttendeeId { get; set; }
        public int MeetingId { get; set; }

        [StringLength(450)]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public bool Accepted { get; set; }
    }
}
