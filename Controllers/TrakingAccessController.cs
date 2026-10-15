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
    public class TrakingAccessController : ControllerBase
    {
        private readonly SuppliersContext _context;

        public TrakingAccessController(SuppliersContext context)
        {
            _context = context;
        }

        // GET: api/TrakingAccess
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TrakingAccess>>> GetTrakingAccess()
        {
            return await _context.TrakingAccess.ToListAsync();
        }

        // Obtengo los Clientes Morosos
        [HttpGet ("get-traking-morosos")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TrakingAccess>>> GetTrakingMorosos()
        {
            return await _context.TrakingAccess.Include(trakingAccess => trakingAccess.GpsPet).Where(trakingAccess => trakingAccess.FinalDate != null && trakingAccess.InitialDate== null).ToListAsync();
        }

        // GET: api/TrakingAccess/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TrakingAccess>> GetTrakingAccess(int id)
        {
            var trakingAccess = await _context.TrakingAccess.FindAsync(id);

            if (trakingAccess == null)
            {
                return NotFound();
            }

            return trakingAccess;
        }

        // PUT: api/TrakingAccess/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTrakingAccess(int id, TrakingAccess trakingAccess)
        {
            if (id != trakingAccess.Id)
            {
                return BadRequest();
            }

            _context.Entry(trakingAccess).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrakingAccessExists(id))
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

        // POST: api/TrakingAccess
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TrakingAccess>> PostTrakingAccess(TrakingAccess trakingAccess)
        {
            _context.TrakingAccess.Add(trakingAccess);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrakingAccess", new { id = trakingAccess.Id }, trakingAccess);
        }

        // DELETE: api/TrakingAccess/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<TrakingAccess>> DeleteTrakingAccess(int id)
        {
            var trakingAccess = await _context.TrakingAccess.FindAsync(id);
            if (trakingAccess == null)
            {
                return NotFound();
            }

            _context.TrakingAccess.Remove(trakingAccess);
            await _context.SaveChangesAsync();

            return trakingAccess;
        }

        private bool TrakingAccessExists(int id)
        {
            return _context.TrakingAccess.Any(e => e.Id == id);
        }
    }
}
