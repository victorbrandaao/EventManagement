using Microsoft.AspNetCore.Mvc;
using EventManagement.Models;
using EventManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Controllers
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

        // GET: api/events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/events/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var evnt = await _context.Events.FindAsync(id);

            if (evnt == null) return NotFound();

            return evnt;
        }

        // POST: api/events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event evnt)
        {
            _context.Events.Add(evnt);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = evnt.Id }, evnt);
        }

        // PUT: api/events/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event evnt)
        {
            if (id != evnt.Id) return BadRequest();

            _context.Entry(evnt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Events.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/events/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var evnt = await _context.Events.FindAsync(id);
            if (evnt == null) return NotFound();

            _context.Events.Remove(evnt);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
