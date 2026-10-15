using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;
using PetsApi.Services;
using PetsApi.Models.Samm;
using System.Text.Json;
using Newtonsoft.Json;
using PetsApi.Models.GPS;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GpsHistoryLocatesController : TMSController
    {
        private readonly SuppliersContext _context;
        private readonly IFlespiService _flespiService;

        public GpsHistoryLocatesController(SuppliersContext context, IFlespiService flespiService) : base(context)
        {
            _context = context;
            _flespiService = flespiService;
        }

        // GET: api/GpsHistoryLocates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GpsHistoryLocate>>> GetGpsHistoryLocate()
        {
            return await _context.GpsHistoryLocate.ToListAsync();
        }

        // POST: api/GpsHistoryLocates/get-history-location
        [HttpPost("get-history-location")]
        public async Task<IActionResult> GetHistoryLocation(Filtro filtro)
        {
            try
            {
                // Manejo seguro de fechas para evitar problemas de formato con la interfaz
                string from = string.IsNullOrEmpty(filtro.from) ? DateTime.UtcNow.ToString("yyyy-MM-dd") : filtro.from;
                string to = string.IsNullOrEmpty(filtro.to) ? DateTime.UtcNow.ToString("yyyy-MM-dd") : filtro.to;

                if (!from.Contains("T")) from += "T00:00:00.000Z";
                if (!to.Contains("T")) to += "T23:59:59.000Z";

                string imei = filtro.imei;

                if (this.IsAdmin || this.IsAuthorizer || this.IsClient)
                {
                    // Llamamos al servicio de Flespi para obtener el historial real
                    var history = await _flespiService.GetHistoryByImeiAsync(imei.ToString(), from, to);
                    return Ok(history);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el historial", error = ex.Message });
            }
        }

        // GET: api/GpsHistoryLocates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GpsHistoryLocate>> GetGpsHistoryLocate(int id)
        {
            var gpsHistoryLocate = await _context.GpsHistoryLocate.FindAsync(id);

            if (gpsHistoryLocate == null)
            {
                return NotFound();
            }

            return gpsHistoryLocate;
        }

        // Obtengo historial de ubicacion de la mascota del cliente
        [HttpGet("get-gps-history/{id}")]
        public async Task<ActionResult<IEnumerable<GpsHistoryLocate>>> GetGpsHistory(int id)
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer)
                {
                    if (this.IsClient)
                    {
                        return await _context.GpsHistoryLocate.Where(gpsHistoryLocate => gpsHistoryLocate.GpsPet.PetId == id && gpsHistoryLocate.GpsPet.ClientId == CurrentClient.Id).ToListAsync();
                    }
                    return Unauthorized();
                }
            return await _context.GpsHistoryLocate.Include(gpsHistoryLocate => gpsHistoryLocate.GpsPet).Where(gpsHistoryLocate => gpsHistoryLocate.GpsPet.PetId == id).ToListAsync();
        }

        // PUT: api/GpsHistoryLocates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGpsHistoryLocate(int id, GpsHistoryLocate gpsHistoryLocate)
        {
            if (id != gpsHistoryLocate.Id)
            {
                return BadRequest();
            }

            _context.Entry(gpsHistoryLocate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GpsHistoryLocateExists(id))
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

        // POST: api/GpsHistoryLocates
        [HttpPost]
        public async Task<ActionResult<GpsHistoryLocate>> PostGpsHistoryLocate(GpsHistoryLocate gpsHistoryLocate)
        {
            _context.GpsHistoryLocate.Add(gpsHistoryLocate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGpsHistoryLocate", new { id = gpsHistoryLocate.Id }, gpsHistoryLocate);
        }

        // DELETE: api/GpsHistoryLocates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GpsHistoryLocate>> DeleteGpsHistoryLocate(int id)
        {
            var gpsHistoryLocate = await _context.GpsHistoryLocate.FindAsync(id);
            if (gpsHistoryLocate == null)
            {
                return NotFound();
            }

            _context.GpsHistoryLocate.Remove(gpsHistoryLocate);
            await _context.SaveChangesAsync();

            return gpsHistoryLocate;
        }

        private bool GpsHistoryLocateExists(int id)
        {
            return _context.GpsHistoryLocate.Any(e => e.Id == id);
        }
    }
}