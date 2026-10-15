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
    public class AntiparasitesController : ControllerBase
    {
        private readonly SuppliersContext _context;

        public AntiparasitesController(SuppliersContext context)
        {
            _context = context;
        }

        // GET: api/Antiparasites
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Antiparasite>>> GetAntiparasites(int petId)
        {
            //return await _context.Antiparasite.ToListAsync();
            return await _context.Antiparasite.Where(antiparasite => antiparasite.PetId == petId).ToListAsync();
        }

        // GET: api/Antiparasites/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Antiparasite>> GetAntiparasite(int id)
        {
            var antiparasite = await _context.Antiparasite.FindAsync(id);

            if (antiparasite == null)
            {
                return NotFound();
            }

            return antiparasite;
        }

        // PUT: api/Antiparasites/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAntiparasite(int id, Antiparasite antiparasite)
        {
            if (id != antiparasite.Id)
            {
                return BadRequest();
            }

            _context.Entry(antiparasite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AntiparasiteExists(id))
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

        // POST: api/Antiparasites
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Antiparasite>> PostAntiparasite(Antiparasite antiparasite)
        {
            _context.Antiparasite.Add(antiparasite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAntiparasite", new { id = antiparasite.Id }, antiparasite);
        }

        // DELETE: api/Antiparasites/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Antiparasite>> DeleteAntiparasite(int id)
        {
            var antiparasite = await _context.Antiparasite.FindAsync(id);
            if (antiparasite == null)
            {
                return NotFound();
            }

            _context.Antiparasite.Remove(antiparasite);
            await _context.SaveChangesAsync();

            return antiparasite;
        }

        private bool AntiparasiteExists(int id)
        {
            return _context.Antiparasite.Any(e => e.Id == id);
        }
    }
}
