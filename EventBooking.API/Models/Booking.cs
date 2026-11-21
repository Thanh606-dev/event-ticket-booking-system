
using Microsoft.EntityFrameworkCore;

namespace EventBooking.API.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int TicketTypeId { get; set; }

        public TicketType TicketType { get; set; } = null!;

        public string CustomerName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        [Precision(18, 2)]
        public decimal UnitPrice { get; set; }

        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        public string? IdempotencyKey { get; set; }
        public string Status { get; set; } = BookingStatuses.Confirmed;
    }
}
