using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using PetsApi.Models;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisingBannersController : ControllerBase
    {
        private readonly SuppliersContext _context;

        public AdvertisingBannersController(SuppliersContext context)
        {
            _context = context;
        }

        // GET: api/AdvertisingBanners
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AdvertisingBanner>>> GetAdvertisingBanner()
        {
            return await _context.AdvertisingBanner.ToListAsync();
        }

        // GET: api/AdvertisingBanners/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AdvertisingBanner>> GetAdvertisingBanner(int id)
        {
            var advertisingBanner = await _context.AdvertisingBanner.FindAsync(id);

            if (advertisingBanner == null)
            {
                return NotFound();
            }

            return advertisingBanner;
        }

        // PUT: api/AdvertisingBanners/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAdvertisingBanner(int id, AdvertisingBanner advertisingBanner)
        {
            if (id != advertisingBanner.Id)
            {
                return BadRequest();
            }

            _context.Entry(advertisingBanner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvertisingBannerExists(id))
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

        // POST: api/AdvertisingBanners
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AdvertisingBanner>> PostAdvertisingBanner(AdvertisingBanner advertisingBanner)
        {
            _context.AdvertisingBanner.Add(advertisingBanner);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdvertisingBanner", new { id = advertisingBanner.Id }, advertisingBanner);
        }

        // DELETE: api/AdvertisingBanners/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<AdvertisingBanner>> DeleteAdvertisingBanner(int id)
        {
            var advertisingBanner = await _context.AdvertisingBanner.FindAsync(id);
            if (advertisingBanner == null)
            {
                return NotFound();
            }

            _context.AdvertisingBanner.Remove(advertisingBanner);
            await _context.SaveChangesAsync();

            return advertisingBanner;
        }

        private bool AdvertisingBannerExists(int id)
        {
            return _context.AdvertisingBanner.Any(e => e.Id == id);
        }
    }
}
