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
    public class TicketsController : TMSController
    {
        private readonly SuppliersContext _context;

        public TicketsController(SuppliersContext context) : base (context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicket()
        {
            return await _context.Ticket
                .Include(ticket => ticket.Client)
                .ThenInclude(client => client.User)
                .Select(ticket => new Ticket(){
                Id = ticket.Id,
                Type = ticket.Type,
                Status = ticket.Status,
                Issue = ticket.Issue,
                Priority = ticket.Priority,
                CreatedDate = ticket.CreatedDate,
                UpdatedDate = ticket.UpdatedDate,
                Client = new Client(){
                    User = new User (){
                        Email = ticket.Client.User.Email,
                        Name = ticket.Client.User.Name
                    },
                },
                ClientId = ticket.ClientId
            })
            .ToListAsync();
        }

        // GET: api/Tickets
        [HttpGet("get-tickets-support-pending")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsSupportPending()
        {
            if(IsClient){
                return await _context.Ticket
                .Include(ticket => ticket.Client)
                .ThenInclude(client => client.User)
                .Include(ticket => ticket.TicketMessages)
                .Where(ticket => ticket.Status == true && ticket.Type == "Support" && ticket.ClientId == CurrentClient.Id)
                .Select(ticket => new Ticket(){
                    Id = ticket.Id,
                    Type = ticket.Type,
                    Status = ticket.Status,
                    Issue = ticket.Issue,
                    Priority = ticket.Priority,
                    CreatedDate = ticket.CreatedDate,
                    UpdatedDate = ticket.UpdatedDate,
                    Client = new Client(){
                        User = new User (){
                            Email = ticket.Client.User.Email,
                            Name = ticket.Client.User.Name
                        },
                    },
                    ClientId = ticket.ClientId,
                    TicketMessages = ticket.TicketMessages,
                })
                .ToListAsync();
            }
            return await _context.Ticket
            .Include(ticket => ticket.Client)
            .ThenInclude(client => client.User)
            .Include(ticket => ticket.TicketMessages)
            .Where(ticket => ticket.Status == true && ticket.Type == "Support")
            .Select(ticket => new Ticket(){
                Id = ticket.Id,
                Type = ticket.Type,
                Status = ticket.Status,
                Issue = ticket.Issue,
                Priority = ticket.Priority,
                CreatedDate = ticket.CreatedDate,
                UpdatedDate = ticket.UpdatedDate,
                Client = new Client(){
                    User = new User (){
                        Email = ticket.Client.User.Email,
                        Name = ticket.Client.User.Name
                    },
                },
                ClientId = ticket.ClientId,
                TicketMessages = ticket.TicketMessages,
            })
            .ToListAsync();
        }

        // GET: api/Tickets
        [HttpGet("get-tickets-claims-pending")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsClaimsPending()
        {
            if(IsClient){
                return await _context.Ticket
                .Include(ticket => ticket.Client)
                .ThenInclude(client => client.User)
                .Include(ticket => ticket.TicketMessages)
                .Where(ticket => ticket.Status == true && ticket.Type == "Claims" && ticket.ClientId == CurrentClient.Id)
                .Select(ticket => new Ticket(){
                    Id = ticket.Id,
                    Type = ticket.Type,
                    Status = ticket.Status,
                    Issue = ticket.Issue,
                    Priority = ticket.Priority,
                    CreatedDate = ticket.CreatedDate,
                    UpdatedDate = ticket.UpdatedDate,
                    Client = new Client(){
                        User = new User (){
                            Email = ticket.Client.User.Email,
                            Name = ticket.Client.User.Name
                        },
                    },
                    ClientId = ticket.ClientId,
                    TicketMessages = ticket.TicketMessages,
                })
                .ToListAsync();
            }
            return await _context.Ticket
            .Include(ticket => ticket.Client)
            .ThenInclude(client => client.User)
            .Include(ticket => ticket.TicketMessages)
            .Where(ticket => ticket.Status == true && ticket.Type == "Claims")
            .Select(ticket => new Ticket(){
                Id = ticket.Id,
                Type = ticket.Type,
                Status = ticket.Status,
                Issue = ticket.Issue,
                Priority = ticket.Priority,
                CreatedDate = ticket.CreatedDate,
                UpdatedDate = ticket.UpdatedDate,
                Client = new Client(){
                    User = new User (){
                        Email = ticket.Client.User.Email,
                        Name = ticket.Client.User.Name
                    },
                },
                ClientId = ticket.ClientId,
                TicketMessages = ticket.TicketMessages,
            })
            .ToListAsync();
        }

        // GET: api/Tickets
        [HttpGet("get-tickets-bug-pending")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsBugPending()
        {
            if(IsClient){
                return await _context.Ticket
                .Include(ticket => ticket.Client)
                .ThenInclude(client => client.User)
                .Include(ticket => ticket.TicketMessages)
                .Where(ticket => ticket.Status == true && ticket.Type == "Bug" && ticket.ClientId == CurrentClient.Id)
                .Select(ticket => new Ticket(){
                    Id = ticket.Id,
                    Type = ticket.Type,
                    Status = ticket.Status,
                    Issue = ticket.Issue,
                    Priority = ticket.Priority,
                    CreatedDate = ticket.CreatedDate,
                    UpdatedDate = ticket.UpdatedDate,
                    Client = new Client(){
                        User = new User (){
                            Email = ticket.Client.User.Email,
                            Name = ticket.Client.User.Name
                        },
                    },
                    ClientId = ticket.ClientId,
                    TicketMessages = ticket.TicketMessages,
                })
                .ToListAsync();
            }
            return await _context.Ticket
            .Include(ticket => ticket.Client)
            .ThenInclude(client => client.User)
            .Include(ticket => ticket.TicketMessages)
            .Where(ticket => ticket.Status == true && ticket.Type == "Bug")
            .Select(ticket => new Ticket(){
                Id = ticket.Id,
                Type = ticket.Type,
                Status = ticket.Status,
                Issue = ticket.Issue,
                Priority = ticket.Priority,
                CreatedDate = ticket.CreatedDate,
                UpdatedDate = ticket.UpdatedDate,
                Client = new Client(){
                    User = new User (){
                        Email = ticket.Client.User.Email,
                        Name = ticket.Client.User.Name
                    },
                },
                ClientId = ticket.ClientId,
                TicketMessages = ticket.TicketMessages,
            })
            .ToListAsync();
        }

        // GET: api/Tickets
        [HttpGet("get-tickets-pending")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsPending()
        {
            if(IsClient){
                return await _context.Ticket
                .Include(ticket => ticket.Client)
                .ThenInclude(client => client.User)
                .Include(ticket => ticket.TicketMessages)
                .Where(ticket => ticket.Status == true && CurrentClient.Id == ticket.ClientId)
                .Select(ticket => new Ticket(){
                    Id = ticket.Id,
                    Type = ticket.Type,
                    Status = ticket.Status,
                    Issue = ticket.Issue,
                    Priority = ticket.Priority,
                    CreatedDate = ticket.CreatedDate,
                    UpdatedDate = ticket.UpdatedDate,
                    Client = new Client(){
                        User = new User (){
                            Email = ticket.Client.User.Email,
                            Name = ticket.Client.User.Name
                        },
                    },
                    ClientId = ticket.ClientId,
                    TicketMessages = ticket.TicketMessages,
                })
                .ToListAsync();
            }
            return await _context.Ticket
            .Include(ticket => ticket.Client)
            .ThenInclude(client => client.User)
            .Include(ticket => ticket.TicketMessages)
            .Where(ticket => ticket.Status == true)
            .Select(ticket => new Ticket(){
                Id = ticket.Id,
                Type = ticket.Type,
                Status = ticket.Status,
                Issue = ticket.Issue,
                Priority = ticket.Priority,
                CreatedDate = ticket.CreatedDate,
                UpdatedDate = ticket.UpdatedDate,
                Client = new Client(){
                    User = new User (){
                        Email = ticket.Client.User.Email,
                        Name = ticket.Client.User.Name
                    },
                },
                ClientId = ticket.ClientId,
                TicketMessages = ticket.TicketMessages,
            })
            .ToListAsync();
        }

        // GET: api/Tickets
        [HttpGet("get-tickets-complete")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsComplete()
        {
            if(IsClient){
                return await _context.Ticket
                .Include(ticket => ticket.Client)
                .ThenInclude(client => client.User)
                .Include(ticket => ticket.TicketMessages)
                .Where(ticket => ticket.Status == false && ticket.ClientId == CurrentClient.Id)
                .Select(ticket => new Ticket(){
                    Id = ticket.Id,
                    Type = ticket.Type,
                    Status = ticket.Status,
                    Issue = ticket.Issue,
                    Priority = ticket.Priority,
                    CreatedDate = ticket.CreatedDate,
                    UpdatedDate = ticket.UpdatedDate,
                    Client = new Client(){
                        User = new User (){
                            Email = ticket.Client.User.Email,
                            Name = ticket.Client.User.Name
                        },
                    },
                    ClientId = ticket.ClientId,
                    TicketMessages = ticket.TicketMessages,
                })
                .ToListAsync();
            }
            return await _context.Ticket
            .Include(ticket => ticket.Client)
            .ThenInclude(client => client.User)
            .Include(ticket => ticket.TicketMessages)
            .Where(ticket => ticket.Status == false)
            .Select(ticket => new Ticket(){
                Id = ticket.Id,
                Type = ticket.Type,
                Status = ticket.Status,
                Issue = ticket.Issue,
                Priority = ticket.Priority,
                CreatedDate = ticket.CreatedDate,
                UpdatedDate = ticket.UpdatedDate,
                Client = new Client(){
                    User = new User (){
                        Email = ticket.Client.User.Email,
                        Name = ticket.Client.User.Name
                    },
                },
                ClientId = ticket.ClientId,
                TicketMessages = ticket.TicketMessages,
            })
            .ToListAsync();
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/Tickets/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // POST: api/Tickets
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
            return new Ticket (){
                Type = ticket.Type,
                Issue = ticket.Issue,
                Priority = ticket.Priority,
                Status = ticket.Status,
                ClientId = ticket.ClientId,
            };
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Ticket>> DeleteTicket(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
