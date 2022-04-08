using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rooms101.Models
{
    public class Meeting
    {
        public Meeting() { }

        [Key]
        public int MeetingId { get; set; }

        [StringLength(450)]
        [MaxLength(450)]
        public string? Owner { get; set; }

        [Required]
        public int MeetingRoomId { get; set; }

        [ForeignKey("MeetingRoomId")]
        public Room MeetingRoom { get; set; } = new Room();

        [StringLength(500)]
        [MaxLength(500)]
        public string? MeetingDescription { get; set; }

        [ForeignKey("MeetingId")]
        public ICollection<MeetingAttendee>? Attendees { get; set; } = new List<MeetingAttendee>();

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateMoment { get; set; } = DateTime.Now;

        public DateTime StartMoment { get; set; }
        public DateTime EndMoment { get; set; }

        [DefaultValue(false)]
        public bool? Cancelled { get; set; } = false;
    }


}
