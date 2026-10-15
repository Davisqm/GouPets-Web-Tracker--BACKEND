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
    public class InventorySimsController : TMSController
    {
        private readonly SuppliersContext _context;

        public InventorySimsController(SuppliersContext context) :base(context)
        {
            _context = context;
        }

        // GET: api/InventorySims
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<InventorySim>>> GetInventorySim()
        {
            return await _context.InventorySim.ToListAsync();
        }

        

        [HttpGet("get-sims-available")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<InventorySim>>> GetSimsAvailable()
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer) 
                        return Unauthorized();
            return await _context.InventorySim.Where(inventorySim=> inventorySim.Status == false && inventorySim.Condition == true).ToListAsync();
        }

        //Obtener sims Usadas
        // [HttpGet("get-sims-used")]
        // public async Task<ActionResult<IEnumerable<InventorySim>>> GetSimsUsed()
        // {
        //     if (!this.IsAdmin)
        //         if (!this.IsAuthorizer) 
        //                 return Unauthorized();
        //     return await _context.InventorySim.Where(inventorySim=> inventorySim.Status == true && inventorySim.Condition == true).ToListAsync();
        // }

        //Obtener Sims Malas
        // [HttpGet("get-bad-sims")]
        // public async Task<ActionResult<IEnumerable<InventorySim>>> GetBadSims()
        // {
        //     if (!this.IsAdmin)
        //         if (!this.IsAuthorizer) 
        //                 return Unauthorized();
        //     return await _context.InventorySim.Where(inventorySim=> inventorySim.Condition == false).ToListAsync();
        // }
        
        // GET: api/InventorySims/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<InventorySim>> GetInventorySim(int id)
        {
            var inventorySim = await _context.InventorySim.FindAsync(id);

            if (inventorySim == null)
            {
                return NotFound();
            }

            return inventorySim;
        }

        // PUT: api/InventorySims/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutInventorySim(int id, InventorySim inventorySim)
        {
            if (id != inventorySim.Id)
            {
                return BadRequest();
            }

            _context.Entry(inventorySim).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventorySimExists(id))
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

        // POST: api/InventorySims
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<InventorySim>> PostInventorySim(InventorySim inventorySim)
        {
            var inventorySim2 = await _context.InventorySim.Where(inventorySim2=>(inventorySim2.LineNumber == inventorySim.LineNumber)).Select(inventorySim2 => new InventorySim {
                Id = inventorySim2.Id,
                LineNumber = inventorySim2.LineNumber,
            }).FirstOrDefaultAsync();

            if (inventorySim2 != null)
            {
                throw new Exception("Ya existe");
            }
            _context.InventorySim.Add(inventorySim);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventorySim", new { id = inventorySim.Id }, inventorySim);
        }

        // DELETE: api/InventorySims/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<InventorySim>> DeleteInventorySim(int id)
        {
            var inventorySim = await _context.InventorySim.FindAsync(id);
            if (inventorySim == null)
            {
                return NotFound();
            }

            _context.InventorySim.Remove(inventorySim);
            await _context.SaveChangesAsync();

            return inventorySim;
        }

        private bool InventorySimExists(int id)
        {
            return _context.InventorySim.Any(e => e.Id == id);
        }
    }
}
