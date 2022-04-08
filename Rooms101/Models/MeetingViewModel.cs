using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rooms101.Models
{
    public class MeetingViewModel
    {
        public MeetingViewModel() { }

        [Key]
        [DisplayName("Meeting ID")]
        public int MeetingId { get; set; }

        [DisplayName("Meeting Owner")]
        public string? Owner { get; set; }
        public string? OwnerId { get; set; }

        [Required]
        [DisplayName("Meeting Room")]
        public int MeetingRoomId { get; set; }

        [DisplayName("Meeting Room")]
        public Room MeetingRoom { get; set; }

        [MaxLength(500)]
        [DisplayName("Description")]
        public string? MeetingDescription { get; set; }

        [DisplayName("Attendees")]
        public ICollection<MeetingAttendee>? Attendees { get; set; } = new List<MeetingAttendee>();

        [DisplayName("Created")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreateMoment { get; set; } = DateTime.Now;

        [DisplayName("Starts")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime StartMoment { get; set; }

        [DisplayName("Ends")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime EndMoment { get; set; }

        [DisplayName("Cancelled")]
        [DefaultValue(false)]
        public bool? Cancelled { get; set; } = false;

        public bool OkToEdit { get; set; } = false;
    }


}
