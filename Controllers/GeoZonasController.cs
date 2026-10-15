using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoZonasController : ControllerBase
    {
        private readonly SuppliersContext _context;

        public GeoZonasController(SuppliersContext context)
        {
            _context = context;
        }

        // GET: api/GeoZonas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeoZona>>> GetGeoZona()
        {
            return await _context.GeoZona.ToListAsync();
        }

        // GET: api/GeoZonas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeoZona>> GetGeoZona(int id)
        {
            var geoZona = await _context.GeoZona.FindAsync(id);

            if (geoZona == null)
            {
                return NotFound();
            }

            return geoZona;
        }

        // PUT: api/GeoZonas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeoZona(int id, GeoZona geoZona)
        {
            if (id != geoZona.Id)
            {
                return BadRequest();
            }

            _context.Entry(geoZona).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeoZonaExists(id))
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


        //Cannot insert explicit value for identity column in table 'GeoZona' when IDENTITY_INSERT is set to OFF.

        // POST: api/GeoZonas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GeoZona>> PostGeoZona(GeoZona geoZona, string geoZonas)
        {

            _context.GeoZona.Add(geoZona);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoZona", new { id = geoZona.Id }, geoZona);
        }
        

        // DELETE: api/GeoZonas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GeoZona>> DeleteGeoZona(int id)
        {
            var geoZona = await _context.GeoZona.FindAsync(id);
            if (geoZona == null)
            {
                return NotFound();
            }

            _context.GeoZona.Remove(geoZona);
            await _context.SaveChangesAsync();

            return geoZona;
        }

        private bool GeoZonaExists(int id)
        {
            return _context.GeoZona.Any(e => e.Id == id);
        }
    }
}
