using Microsoft.AspNetCore.Mvc;
using PetsApi.Services;
using System.Threading.Tasks;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlespiTestController : ControllerBase
    {
        private readonly IFlespiService _flespiService;

        public FlespiTestController(
            IFlespiService flespiService)
        {
            _flespiService = flespiService;
        }

        // ============================================================
        // PRUEBA DEL HISTORIAL FLESPI
        // ============================================================
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory(
            string imei,
            string from,
            string to)
        {
            var history =
                await _flespiService.GetHistoryByImeiAsync(
                    imei,
                    from,
                    to);

            return Ok(history);
        }
    }
}