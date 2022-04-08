using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rooms101.Models
{
    public class RoomViewModel
    {
        public RoomViewModel()
        {
        }

        [DisplayName("Room ID")]
        public int MeetingRoomId { get; set; }

        [DisplayName("Room Name")]
        [Required]
        [MaxLength(50)]
        public string MeetingRoomName { get; set; } = "";

        [DisplayName("Room Description")]
        [MaxLength(250)]
        public string MeetingRoomDescription { get; set; } = "";

        [DisplayName("Display Colour")]
        [MaxLength(50)]
        public string BackgroundColour { get; set; } = "";


        [DisplayName("In Service")]
        [Required]
        [DefaultValue(true)]
        public bool Active { get; set; }
    }
}
