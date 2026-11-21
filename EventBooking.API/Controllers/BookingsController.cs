
using EventBooking.API.Data;
using EventBooking.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EventBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return Ok(ToResponse(booking));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            [FromBody] CreateBookingRequest request)
        {
            if (request.TicketTypeId <= 0 ||
                request.Quantity <= 0 ||
                request.Quantity > 100 ||
                string.IsNullOrWhiteSpace(request.CustomerName) ||
                request.CustomerName.Length > 200 ||
                !Guid.TryParse(request.IdempotencyKey, out var guid))
            {
                return BadRequest("Thong tin dat ve khong hop le.");
            }

            // Chuan hoa Idempotency Key
            var key = guid.ToString("D");
            var customerName = request.CustomerName.Trim();

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            // Khoa cung key tren toan bo cac API instance
            var lockResult = new SqlParameter(
                "@lockResult", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var resource = new SqlParameter(
                "@resource", SqlDbType.NVarChar, 255)
            {
                Value = "Booking:" + key
            };

            await _context.Database.ExecuteSqlRawAsync(
                @"EXEC @lockResult = sys.sp_getapplock
                    @Resource = @resource,
                    @LockMode = 'Exclusive',
                    @LockOwner = 'Transaction',
                    @LockTimeout = 10000;",
                lockResult,
                resource
            );

            if ((int)lockResult.Value < 0)
            {
                return StatusCode(503,
                    "He thong dang ban. Vui long thu lai.");
            }

            // Kiem tra yeu cau da duoc xu ly chua
            var existing = await _context.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    b => b.IdempotencyKey == key);

            if (existing != null)
            {
                // Khong cho dung cung key voi du lieu khac
                if (existing.TicketTypeId != request.TicketTypeId ||
                    existing.Quantity != request.Quantity ||
                    existing.CustomerName != customerName)
                {
                    return Conflict(
                        "Idempotency Key da duoc su dung.");
                }

                // Tra ve don cu, khong tru them ve
                return Ok(ToResponse(existing));
            }

            var ticket = await _context.TicketTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    t => t.Id == request.TicketTypeId);

            if (ticket == null)
                return NotFound("Loai ve khong ton tai.");

            // Tru ve an toan bang SQL UPDATE co dieu kien
            var updatedRows = await _context.TicketTypes
                .Where(t =>
                    t.Id == request.TicketTypeId &&
                    t.SoldQuantity <=
                        t.TotalQuantity - request.Quantity)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(
                        t => t.SoldQuantity,
                        t => t.SoldQuantity + request.Quantity));

            if (updatedRows == 0)
                return Conflict("Khong du ve de dat.");

            var booking = new Booking
            {
                TicketTypeId = request.TicketTypeId,
                CustomerName = customerName,
                Quantity = request.Quantity,
                UnitPrice = ticket.Price,
                TotalPrice = ticket.Price * request.Quantity,
                CreatedAt = DateTime.UtcNow,
                Status = BookingStatuses.Confirmed,

                IdempotencyKey = key
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return CreatedAtAction(
                nameof(GetBooking),
                new { id = booking.Id },
                ToResponse(booking));
        }

        private static object ToResponse(Booking booking)
        {
            return new
            {
                booking.Id,
                booking.TicketTypeId,
                booking.CustomerName,
                booking.Quantity,
                booking.UnitPrice,
                booking.TotalPrice,
                booking.CreatedAt,
                booking.Status,
                booking.IdempotencyKey
            };
        }
    }

    public class CreateBookingRequest
    {
        public int TicketTypeId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public string IdempotencyKey { get; set; } = string.Empty;
    }
}
