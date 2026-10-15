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
    public class VaccinesController : ControllerBase
    {
        private readonly SuppliersContext _context;

        public VaccinesController(SuppliersContext context)
        {
            _context = context;
        }

        // GET: api/Vaccines
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Vaccine>>> GetVaccines(int petId)
        {
            return await _context.Vaccine.Where(vaccine => vaccine.PetId == petId).ToListAsync();
        }

        // GET: api/Vaccines/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Vaccine>> GetVaccine(int id)
        {
            var vaccine = await _context.Vaccine.FindAsync(id);

            if (vaccine == null)
            {
                return NotFound();
            }

            return vaccine;
        }

        // PUT: api/Vaccines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutVaccine(int id, Vaccine vaccine)
        {
            if (id != vaccine.Id)
            {
                return BadRequest();
            }

            _context.Entry(vaccine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VaccineExists(id))
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

        // POST: api/Vaccines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Vaccine>> PostVaccine(Vaccine vaccine)
        {
            _context.Vaccine.Add(vaccine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVaccine", new { id = vaccine.Id }, vaccine);
        }

        // DELETE: api/Vaccines/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Vaccine>> DeleteVaccine(int id)
        {
            var vaccine = await _context.Vaccine.FindAsync(id);
            if (vaccine == null)
            {
                return NotFound();
            }

            _context.Vaccine.Remove(vaccine);
            await _context.SaveChangesAsync();

            return vaccine;
        }

        private bool VaccineExists(int id)
        {
            return _context.Vaccine.Any(e => e.Id == id);
        }
    }
}
