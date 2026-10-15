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
    public class RatesController : ControllerBase
    {
        private readonly SuppliersContext _context;

        public RatesController(SuppliersContext context)
        {
            _context = context;
        }

        // GET: api/Rate
        [HttpGet("get-rate-all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Rate>>> GetRateAll()
        {
            var query = _context.Rate
            .Include(rate => rate.Specie)  
            .Select(rate => new Rate {
                Id = rate.Id,
                Name = rate.Name,
                Specie = rate.Specie,
                SpecieId = rate.SpecieId,
            });
            return await query.ToListAsync();
        }

        // GET: api/Rates
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Rate>>> GetRate()
        {
            return await _context.Rate.ToListAsync();
        }

        // GET: api/Rates/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Rate>> GetRate(int id)
        {
            var rate = await _context.Rate.FindAsync(id);

            if (rate == null)
            {
                return NotFound();
            }

            return rate;
        }


        

        // PUT: api/Rates/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutRate(int id, Rate rate)
        {
            if (id != rate.Id)
            {
                return BadRequest();
            }

            _context.Entry(rate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RateExists(id))
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

        // POST: api/Rates
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Rate>> PostRate(Rate rate)
        {
            var rate2 = await _context.Rate.Where(rate2=>(rate2.Name.ToLower()).Equals(rate.Name.ToLower()) && rate2.SpecieId == rate.SpecieId).Select(rate2 => new Rate {
                Id = rate2.Id,
                Name = rate2.Name,
                SpecieId = rate2.SpecieId,
            }).FirstOrDefaultAsync();

            if (rate2 != null)
            {
                throw new Exception("Ya existe");
            }

            _context.Rate.Add(rate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRate", new { id = rate.Id }, rate);
        }

        // DELETE: api/Rates/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Rate>> DeleteRate(int id)
        {
            var rate = await _context.Rate.FindAsync(id);
            if (rate == null)
            {
                return NotFound();
            }

            _context.Rate.Remove(rate);
            await _context.SaveChangesAsync();

            return rate;
        }

        private bool RateExists(int id)
        {
            return _context.Rate.Any(e => e.Id == id);
        }
    }
}
