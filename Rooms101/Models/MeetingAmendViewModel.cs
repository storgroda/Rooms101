using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rooms101.Models
{
    public class MeetingAmendViewModel
    {
        public MeetingAmendViewModel() { }


        [DisplayName("ID")]
        public int MeetingId { get; set; }

        [DisplayName("Meeting Owner")]
        [MaxLength(450)]
        public string? Owner { get; set; }

        [DisplayName("Meeting Room")]
        [Required]
        public int MeetingRoomId { get; set; }

        [DisplayName("Description")]
        [MaxLength(500)]
        public string MeetingDescription { get; set; }

        [DisplayName("Attendees")]
        public string[] AttendeeId { get; set; }

        [DisplayName("Start")]
        public DateTime StartMoment { get; set; }

        [DisplayName("End")]
        public DateTime EndMoment { get; set; }
        public bool OkToEdit { get; set; } = true;
    }


}
