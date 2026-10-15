using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;
using PetsApi.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PetsApi.Controllers
{
    [ApiController]
    [Route("api/gps")]
    [Authorize]
    public class GpsController : ControllerBase
    {
        private readonly IFlespiService _flespi;
        private readonly SuppliersContext _context;

        public GpsController(
            IFlespiService flespi,
            SuppliersContext context)
        {
            _flespi = flespi;
            _context = context;
        }

        // Lista todos los dispositivos registrados en Flespi
        [HttpGet("devices")]
        public async Task<IActionResult> Devices()
        {
            var result = await _flespi.GetDevicesAsync();

            return Content(result, "application/json");
        }

        // Obtiene la última posición usando el DeviceId de Flespi
        [HttpGet("position/{deviceId}")]
        public async Task<IActionResult> Position(long deviceId)
        {
            var gps = await _flespi.GetLastPositionByDeviceIdAsync(deviceId);

            if (gps == null)
                return NotFound("No se encontró la posición.");

            return Ok(gps);
        }

        // Obtiene la posición del GPS asociado a una mascota
        [HttpGet("pet/{petId}")]
        public async Task<IActionResult> GetPetPosition(int petId)
        {
            var gpsPet = await _context.GpsPet
                .Include(x => x.DeviceTraking)
                .FirstOrDefaultAsync(x => x.PetId == petId);

            if (gpsPet == null)
                return NotFound("La mascota no tiene un GPS asociado.");

            // 1. Buscar por DeviceId de Flespi
            if (gpsPet.DeviceTraking != null &&
                gpsPet.DeviceTraking.FlespiDeviceId.HasValue)
            {
                var gps = await _flespi.GetLastPositionByDeviceIdAsync(
                    gpsPet.DeviceTraking.FlespiDeviceId.Value);

                if (gps == null)
                    return NotFound("No se encontró la posición.");

                return Ok(gps);
            }

            // 2. Si no existe DeviceId, buscar por IMEI
            if (gpsPet.DeviceTraking != null &&
                !string.IsNullOrWhiteSpace(gpsPet.DeviceTraking.Imei))
            {
                var gps = await _flespi.GetPositionByImeiAsync(
                    gpsPet.DeviceTraking.Imei);

                return Ok(gps);
            }

            return BadRequest("El dispositivo no tiene configurado FlespiDeviceId ni IMEI.");
        }

        // ============================================================
        // HISTORIAL DE RECORRIDO DE UNA MASCOTA
        // Ejemplo: GET api/gps/pet/5/history?from=2026-10-01&to=2026-10-06
        // ============================================================
        [HttpGet("pet/{petId}/history")]
        public async Task<IActionResult> GetPetHistory(
            int petId,
            [FromQuery] string from,
            [FromQuery] string to)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                return BadRequest("Debes enviar los parámetros 'from' y 'to'.");

            var gpsPet = await _context.GpsPet
                .Include(x => x.DeviceTraking)
                .FirstOrDefaultAsync(x => x.PetId == petId);

            if (gpsPet == null)
                return NotFound("La mascota no tiene un GPS asociado.");

            if (gpsPet.DeviceTraking == null ||
                string.IsNullOrWhiteSpace(gpsPet.DeviceTraking.Imei))
                return BadRequest("El dispositivo de la mascota no tiene IMEI configurado.");

            return await GetHistoryResult(gpsPet.DeviceTraking.Imei, from, to);
        }

        // ============================================================
        // HISTORIAL DE RECORRIDO POR IMEI
        // Ejemplo: GET api/gps/history/867730056432143?from=2026-10-01&to=2026-10-06
        // ============================================================
        [HttpGet("history/{imei}")]
        public async Task<IActionResult> GetHistoryByImei(
            string imei,
            [FromQuery] string from,
            [FromQuery] string to)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                return BadRequest("Debes enviar los parámetros 'from' y 'to'.");

            return await GetHistoryResult(imei, from, to);
        }

        private async Task<IActionResult> GetHistoryResult(string imei, string from, string to)
        {
            try
            {
                var history = await _flespi.GetHistoryByImeiAsync(imei, from, to);
                return Ok(history);
            }
            catch (ArgumentException ex)
            {
                // Fechas inválidas
                return BadRequest(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                // Falla al consultar Flespi
                return StatusCode(502, $"Error consultando Flespi: {ex.Message}");
            }
        }
    }
}