using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketMessagesController : TMSController
    {
        private readonly SuppliersContext _context;

        public TicketMessagesController(SuppliersContext context) : base(context)
        {
            _context = context;
        }

        // GET: api/TicketMessages
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TicketMessage>>> GetTicketMessage()
        {
            return await _context.TicketMessage.ToListAsync();
        }

        // GET: api/TicketMessages/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TicketMessage>> GetTicketMessage(int id)
        {
            var ticketMessage = await _context.TicketMessage.FindAsync(id);

            if (ticketMessage == null)
            {
                return NotFound();
            }

            return ticketMessage;
        }

        // PUT: api/TicketMessages/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTicketMessage(int id, TicketMessage ticketMessage)
        {
            if (id != ticketMessage.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticketMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketMessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TicketMessages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TicketMessage>> PostTicketMessage(TicketMessage ticketMessage)
        {
            _context.TicketMessage.Add(ticketMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicketMessage", new { id = ticketMessage.Id }, ticketMessage);
        }

        // DELETE: api/TicketMessages/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<TicketMessage>> DeleteTicketMessage(int id)
        {
            var ticketMessage = await _context.TicketMessage.FindAsync(id);
            if (ticketMessage == null)
            {
                return NotFound();
            }

            _context.TicketMessage.Remove(ticketMessage);
            await _context.SaveChangesAsync();

            return ticketMessage;
        }

        private bool TicketMessageExists(int id)
        {
            return _context.TicketMessage.Any(e => e.Id == id);
        }
    }
}
