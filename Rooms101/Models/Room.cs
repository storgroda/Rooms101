using System.ComponentModel.DataAnnotations;

namespace Rooms101.Models
{
    public class Room
    {
        public Room()
        {
        }

        [Key]
        public int MeetingRoomId { get; set; }

        [MaxLength(50)]
        public string MeetingRoomName { get; set; } = "";

        [MaxLength(250)]
        public string MeetingRoomDescription { get; set; } = "";

        [StringLength(50)]
        public string? BackgroundColour { get; set; }

        [Required]
        public bool Active { get; set; } = true;
    }
}
