
using EventBooking.API.Data;
using EventBooking.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .AsNoTracking()
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/Events/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _context.Events
                .FindAsync(id);

            if (eventItem == null)
                return NotFound();

            return Ok(eventItem);
        }

        // POST: api/Events
        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event eventItem)
        {
            if (string.IsNullOrWhiteSpace(eventItem.Name)
                || eventItem.Price < 0
                || eventItem.TotalTickets < 0)
            {
                return BadRequest("Du lieu su kien khong hop le.");
            }

            eventItem.Id = 0;

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = eventItem.Id },
                eventItem
            );
        }

        // PUT: api/Events/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEvent(
            int id, Event input)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(input.Name)
                || input.Price < 0
                || input.TotalTickets < 0)
                return BadRequest("Du lieu su kien khong hop le.");

            eventItem.Name = input.Name;
            eventItem.Description = input.Description;
            eventItem.Location = input.Location;
            eventItem.StartTime = input.StartTime;
            eventItem.Price = input.Price;
            eventItem.TotalTickets = input.TotalTickets;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Events/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return NotFound();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
