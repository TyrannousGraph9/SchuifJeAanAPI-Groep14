using System.ComponentModel.DataAnnotations;

namespace SchuifJeAanAPI.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public int UserId { get; set; }  // Owner of the event
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int MaxGuestCount { get; set; }
        public List<int> JoinedUserIds { get; set; } = new List<int>();
    }
}
