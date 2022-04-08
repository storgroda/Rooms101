using System.ComponentModel.DataAnnotations;

namespace Rooms101.Models
{
    public class CalendarEvent
    {
        public int id { get; set; }
        public string title { get; set; }

        public string description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
        public DateTime start { get; set; }
        
        public DateTime end { get; set; }
        public string url { get; set; }
    }
}
