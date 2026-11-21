
using EventBooking.API.Data;
using EventBooking.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventBooking.API.Controllers
{
    [ApiController]
    [Route("api/events/{eventId:int}/ticket-types")]
    public class TicketTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketTypesController(AppDbContext context)
        {
            _context = context;
        }

        // Lay danh sach loai ve cua su kien
        [HttpGet]
        public async Task<IActionResult> GetTicketTypes(int eventId)
        {
            var tickets = await _context.TicketTypes
                .AsNoTracking()
                .Where(t => t.EventId == eventId)
                .Select(t => new
                {
                    t.Id,
                    t.EventId,
                    t.Name,
                    t.Price,
                    t.TotalQuantity,
                    t.SoldQuantity,
                    RemainingQuantity =
                        t.TotalQuantity - t.SoldQuantity
                })
                .ToListAsync();

            return Ok(tickets);
        }

        // Lay chi tiet mot loai ve
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTicketType(
            int eventId, int id)
        {
            var ticket = await _context.TicketTypes
                .AsNoTracking()
                .Where(t => t.EventId == eventId && t.Id == id)
                .Select(t => new
                {
                    t.Id,
                    t.EventId,
                    t.Name,
                    t.Price,
                    t.TotalQuantity,
                    t.SoldQuantity,
                    RemainingQuantity =
                        t.TotalQuantity - t.SoldQuantity
                })
                .FirstOrDefaultAsync();

            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        // Tao loai ve moi
        [HttpPost]
        public async Task<IActionResult> CreateTicketType(
            int eventId,
            [FromBody] CreateTicketTypeRequest request)
        {
            var eventExists = await _context.Events
                .AnyAsync(e => e.Id == eventId);

            if (!eventExists)
                return NotFound("Su kien khong ton tai.");

            if (string.IsNullOrWhiteSpace(request.Name)
                || request.Price < 0
                || request.TotalQuantity <= 0)
            {
                return BadRequest("Thong tin ve khong hop le.");
            }

            var ticket = new TicketType
            {
                EventId = eventId,
                Name = request.Name.Trim(),
                Price = request.Price,
                TotalQuantity = request.TotalQuantity,
                SoldQuantity = 0
            };

            _context.TicketTypes.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTicketType),
                new { eventId, id = ticket.Id },
                new
                {
                    ticket.Id,
                    ticket.EventId,
                    ticket.Name,
                    ticket.Price,
                    ticket.TotalQuantity,
                    ticket.SoldQuantity
                }
            );
        }
    }

    public class CreateTicketTypeRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int TotalQuantity { get; set; }
    }
}
