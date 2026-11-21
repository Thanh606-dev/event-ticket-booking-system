using Microsoft.EntityFrameworkCore;
namespace EventBooking.API.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }

        public int TotalTickets { get; set; }
    }
}
