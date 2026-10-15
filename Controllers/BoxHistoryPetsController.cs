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
    public class BoxHistoryPetsController : ControllerBase
    {
        private readonly SuppliersContext _context;

        public BoxHistoryPetsController(SuppliersContext context)
        {
            _context = context;
        }

        // GET: api/BoxHistoryPets
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BoxHistoryPet>>> GetBoxHistoryPet()
        {
            return await _context.BoxHistoryPet.ToListAsync();
        }

        // GET: api/BoxHistoryPets/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BoxHistoryPet>> GetBoxHistoryPet(int id)
        {
            var boxHistoryPet = await _context.BoxHistoryPet.FindAsync(id);

            if (boxHistoryPet == null)
            {
                return NotFound();
            }

            return boxHistoryPet;
        }

        // PUT: api/BoxHistoryPets/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBoxHistoryPet(int id, BoxHistoryPet boxHistoryPet)
        {
            if (id != boxHistoryPet.Id)
            {
                return BadRequest();
            }

            _context.Entry(boxHistoryPet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoxHistoryPetExists(id))
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

        // POST: api/BoxHistoryPets
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BoxHistoryPet>> PostBoxHistoryPet(BoxHistoryPet boxHistoryPet)
        {
            _context.BoxHistoryPet.Add(boxHistoryPet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoxHistoryPet", new { id = boxHistoryPet.Id }, boxHistoryPet);
        }

        // DELETE: api/BoxHistoryPets/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<BoxHistoryPet>> DeleteBoxHistoryPet(int id)
        {
            var boxHistoryPet = await _context.BoxHistoryPet.FindAsync(id);
            if (boxHistoryPet == null)
            {
                return NotFound();
            }

            _context.BoxHistoryPet.Remove(boxHistoryPet);
            await _context.SaveChangesAsync();

            return boxHistoryPet;
        }

        private bool BoxHistoryPetExists(int id)
        {
            return _context.BoxHistoryPet.Any(e => e.Id == id);
        }
    }
}
