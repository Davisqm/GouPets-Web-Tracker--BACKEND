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
    public class SpeciesController : ControllerBase
    {
        private readonly SuppliersContext _context;

        public SpeciesController(SuppliersContext context)
        {
            _context = context;
        }

        // GET: api/Species
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Specie>>> GetSpecie()
        {
            return await _context.Specie.ToListAsync();
        }

        // GET: api/Species
        [HttpGet("get-specie-all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Specie>>> GetSpecieAll()
        {
            var query = _context.Specie
            .Include(specie => specie.Rate)  
            .Select(specie => new Specie {
                Id = specie.Id,
                Name = specie.Name,
                Rate = specie.Rate,
            });
            return await query.ToListAsync();
        }


        
        // GET: api/Species/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Specie>> GetSpecie(int id)
        {
            var specie = await _context.Specie.FindAsync(id);

            if (specie == null)
            {
                return NotFound();
            }

            return specie;
        }

        // PUT: api/Species/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutSpecie(int id, Specie specie)
        {
            if (id != specie.Id)
            {
                return BadRequest();
            }

            _context.Entry(specie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecieExists(id))
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

        // POST: api/Species
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Specie>> PostSpecie(Specie specie)
        {
            var specie2 = await _context.Specie.Where(specie2=>specie2.Name == specie.Name).Select(specie2 => new Specie {
                Id = specie2.Id,
                Name = specie2.Name,
            }).FirstOrDefaultAsync();

            if (specie2 != null)
            {
                throw new Exception("Ya existe");
            }
            _context.Specie.Add(specie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpecie", new { id = specie.Id }, specie);
        }

        // DELETE: api/Species/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Specie>> DeleteSpecie(int id)
        {
            try {
                var specie = await _context.Specie.FindAsync(id);
                if (specie == null)
                {
                    return NotFound();
                }

                _context.Specie.Remove(specie);
                await _context.SaveChangesAsync();

                return specie;

            } catch {
                throw new Exception("Debe eliminar primero todas las razas asociadas a esta especie.");
            }
            
        }

        private bool SpecieExists(int id)
        {
            return _context.Specie.Any(e => e.Id == id);
        }
    }
}
