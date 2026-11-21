
using Microsoft.EntityFrameworkCore;

namespace EventBooking.API.Models
{
    public class TicketType
    {
        public int Id { get; set; }

        // Su kien ma loai ve nay thuoc ve
        public int EventId { get; set; }

        public Event Event { get; set; } = null!;

        // Vi du: Standard, VIP
        public string Name { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal Price { get; set; }

        // Tong so ve
        public int TotalQuantity { get; set; }

        // So ve da ban
        public int SoldQuantity { get; set; } = 0;
    }
}
