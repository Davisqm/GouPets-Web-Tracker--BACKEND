using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;
using PetsApi.Services;
using PetsApi.Models.Samm;
using Microsoft.AspNetCore.Authorization;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GpsPetsController : TMSController
    {
        private readonly SuppliersContext _context;
        private readonly ISammService _sammService;

        public GpsPetsController(SuppliersContext context, ISammService sammService) : base(context) 
        {
            _context = context;
            _sammService = sammService;
        }

        // GET: api/GpsPets
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GpsPet>>> GetGpsPet()
        {
            return await _context.GpsPet.ToListAsync();
        }

        //Obtengo los GpsPets Disponibles
        [HttpGet("get-gpspets-available")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GpsPet>>> GetGpsPetsAvailable()
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer) {
                    if(this.IsClient){
                        return await _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking)
                        .Include(gpsPet => gpsPet.InventorySim)
                        .Where(gpspet=> gpspet.ClientId == CurrentClient.Id)
                        .Select(gpsPet => new GpsPet  {
                            Id = gpsPet.Id,
                            ClientId = gpsPet.ClientId,
                            PetId = gpsPet.PetId,
                            DeviceTraking = gpsPet.DeviceTraking,
                            DeviceTrakingId = gpsPet.DeviceTrakingId,
                            InventorySim = gpsPet.InventorySim,
                            InventorySimId = gpsPet.InventorySimId,
                            Status = gpsPet.Status,
                        })
                        .ToListAsync();
                    }else{
                        return Unauthorized();
                    }
                }
            return await _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking)
            .Include(gpsPet => gpsPet.InventorySim)
            .Where(gpspet => gpspet.ClientId == null)
            .Select(gpsPet => new GpsPet {
                Id = gpsPet.Id,
                ClientId = gpsPet.ClientId,
                DeviceTraking = gpsPet.DeviceTraking,
                DeviceTrakingId = gpsPet.DeviceTrakingId,
                InventorySim = gpsPet.InventorySim,
                InventorySimId = gpsPet.InventorySimId,
                PetId = gpsPet.PetId,
                Status = gpsPet.Status,
                })
            .ToListAsync();
        }

        [HttpGet("get-gpspets-location/{imei}")]
        [Authorize]
        public async Task<ActionResult<Position>> GetGpspetsLocation(string imei)
        {
            if (this.IsAdmin || this.IsAuthorizer || this.IsClient){
                return await _sammService.GetDeviceById(imei.ToString());
            }
            else{
                return null;
            }
        }



        //Obtengo los GpsPets Usados
        [HttpGet("get-gpspets-used")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GpsPet>>> GetGpspetsUsed()
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer)
                    return Unauthorized();
            var query = _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking).Include(gpsPet => gpsPet.InventorySim).Include(gpsPet => gpsPet.Client).Include(gpsPet => gpsPet.Client.User).Where(gpsPet => gpsPet.ClientId != null).Select(gpsPet => new GpsPet {
                Id = gpsPet.Id,
                ClientId = gpsPet.ClientId,
                Client = gpsPet.Client,
                DeviceTraking = gpsPet.DeviceTraking,
                DeviceTrakingId = gpsPet.DeviceTrakingId,
                InventorySim = gpsPet.InventorySim,
                InventorySimId = gpsPet.InventorySimId,
                Status = gpsPet.Status,
            });
            return await query.ToListAsync();
        }

        //Obtengo los GpsPets de cada cliente loggeado
        [HttpGet("get-gpspets-client")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GpsPet>>> GetGpsPetsClient()
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer){
                    if(this.IsClient){
                        var listaGpsPets = await _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking)
                         .Include(gpsPet => gpsPet.Pet)
                         .Include(gpsPet => gpsPet.InventorySim)
                         .Where(gpsPet => gpsPet.ClientId == CurrentClient.Id)
                         .Select(gpsPet => new GpsPet
                         {
                             Id = gpsPet.Id,
                             ClientId = gpsPet.ClientId,
                             DeviceTraking = gpsPet.DeviceTraking,
                             DeviceTrakingId = gpsPet.DeviceTrakingId,
                             InventorySim = gpsPet.InventorySim,
                             InventorySimId = gpsPet.InventorySimId,
                             Pet = new Pet
                             {
                                 Id = gpsPet.Pet.Id,
                                 Name = gpsPet.Pet == null ? null : gpsPet.Pet.Name
                             },
                             Status = gpsPet.Status,
                         }).ToListAsync();

                        foreach (GpsPet dato in listaGpsPets)
                        {
                            if (dato.DeviceTraking != null && !string.IsNullOrEmpty(dato.DeviceTraking.Imei))
                            {
                                Position position = await _sammService.GetDeviceById(dato.DeviceTraking.Imei.ToString());
                                if (position != null && position.attributes != null)
                                {
                                    dato.DeviceTraking.BatteryLevel = (!string.IsNullOrEmpty(position.attributes.batteryLevel)) ? position.attributes.batteryLevel : "NaN";
                                }
                                else
                                {
                                    dato.DeviceTraking.BatteryLevel = "NaN";
                                }
                            }
                            else if (dato.DeviceTraking != null)
                            {
                                dato.DeviceTraking.BatteryLevel = "NaN";
                            }
                        }
                        return listaGpsPets;
                    }
                    return Unauthorized();
                } 
                var query1 = _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking).Include(gpsPet => gpsPet.InventorySim).Include(gpsPet => gpsPet.Client.User).Select(gpsPet => new GpsPet {
                Id = gpsPet.Id,
                DeviceTraking = gpsPet.DeviceTraking,
                DeviceTrakingId = gpsPet.DeviceTrakingId,
                Status = gpsPet.Status,
                });
                return await query1.ToListAsync();
        }

        //Obtengo los GpsPets de un cliente en especifico por id
        [HttpGet("get-gpspet-client/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GpsPet>>> GetGpspetClient(int id)
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer)
                    return Unauthorized();
            return await _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking)
            .Include(gpsPet => gpsPet.InventorySim)
            .Where(gpsPet => gpsPet.ClientId == id)
            .Select(gpsPet => new GpsPet {
                Id = gpsPet.Id,
                ClientId = gpsPet.ClientId,
                DeviceTraking = gpsPet.DeviceTraking,
                DeviceTrakingId = gpsPet.DeviceTrakingId,
                InventorySim = gpsPet.InventorySim,
                InventorySimId = gpsPet.InventorySimId,
                Pet = new Pet {
                    Id = gpsPet.Pet.Id,
                    Name = gpsPet.Pet == null ? null : gpsPet.Pet.Name
                },
                Status = gpsPet.Status,
                }).ToListAsync();
        }

        //Obtengo los GpsPets con mascota de cada cliente
        [HttpGet("get-gpspet-client-pets")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GpsPet>>> GetGpsPetsClientPets()
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer){
                    if (this.IsClient){
                        var listaGpsPetsConMascota = await _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking)
                        .Include(gpsPet => gpsPet.InventorySim)
                        .Include(gpsPet => gpsPet.Pet)
                        .Where(gpsPet => gpsPet.PetId != null && gpsPet.ClientId == CurrentClient.Id)
                        .Select(gpsPet => new GpsPet
                        {
                            Id = gpsPet.Id,
                            ClientId = gpsPet.ClientId,
                            DeviceTraking = gpsPet.DeviceTraking,
                            DeviceTrakingId = gpsPet.DeviceTrakingId,
                            InventorySim = gpsPet.InventorySim,
                            InventorySimId = gpsPet.InventorySimId,
                            PetId = gpsPet.PetId,
                            Pet = new Pet
                            {
                                Name = gpsPet.Pet == null ? null : gpsPet.Pet.Name,
                                Id = gpsPet.Pet.Id,
                            },
                            Status = gpsPet.Status,
                        }).ToListAsync();

                        foreach (GpsPet dato in listaGpsPetsConMascota)
                        {
                            if (dato.DeviceTraking != null && !string.IsNullOrEmpty(dato.DeviceTraking.Imei))
                            {
                                Position position = await _sammService.GetDeviceById(dato.DeviceTraking.Imei.ToString());
                                if (position != null && position.attributes != null)
                                {
                                    dato.DeviceTraking.BatteryLevel = (!string.IsNullOrEmpty(position.attributes.batteryLevel)) ? position.attributes.batteryLevel : "NaN";
                                }
                                else
                                {
                                    dato.DeviceTraking.BatteryLevel = "NaN";
                                }
                            }
                            else if (dato.DeviceTraking != null)
                            {
                                dato.DeviceTraking.BatteryLevel = "NaN";
                            }
                        }
                        return listaGpsPetsConMascota;
                    }
                    return Unauthorized();
                }
            return await _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking)
                .Include(gpsPet => gpsPet.InventorySim)
                .Include(gpsPet => gpsPet.Pet)
                .Where(gpsPet => gpsPet.PetId != null)
                    .Select(gpsPet => new GpsPet {
                    Id = gpsPet.Id,
                    ClientId = gpsPet.ClientId,
                    DeviceTraking = gpsPet.DeviceTraking,
                    DeviceTrakingId = gpsPet.DeviceTrakingId,
                    InventorySim = gpsPet.InventorySim,
                    InventorySimId = gpsPet.InventorySimId,
                    PetId = gpsPet.PetId,
                    Pet = new Pet {
                        Name = gpsPet.Pet == null ? null : gpsPet.Pet.Name,
                        Id =  gpsPet.Pet.Id,
                    },
                    Status = gpsPet.Status,
            }).ToListAsync();
        }

        //Obtengo los GpsPets sin mascota de cada cliente
        [HttpGet("get-gpspet-client-no-pets")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GpsPet>>> GetGpsPetsclientNoPets()
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer){
                    if (this.IsClient){
                        return await _context.GpsPet
                        .Include(gpsPet => gpsPet.DeviceTraking)
                        .Include(gpsPet => gpsPet.InventorySim)
                        .Where(gpsPet => gpsPet.PetId == null && gpsPet.ClientId == CurrentClient.Id)
                        .Select(gpsPet => new GpsPet {
                            Id = gpsPet.Id,
                            ClientId = gpsPet.ClientId,
                            DeviceTraking = gpsPet.DeviceTraking,
                            DeviceTrakingId = gpsPet.DeviceTrakingId,
                            InventorySim = gpsPet.InventorySim,
                            InventorySimId = gpsPet.InventorySimId,
                            PetId = gpsPet.PetId,
                            Status = gpsPet.Status,
                            }).ToListAsync();
                    }
                    return Unauthorized();
                }
            return await _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking).Include(gpsPet => gpsPet.InventorySim).Where(gpsPet => gpsPet.PetId == null).ToListAsync();
        }

        //Obtengo los GpsPets con mascota de cada cliente
        [HttpGet("get-gpspet-pets/{id}")]
        [Authorize]
        public async Task<ActionResult<GpsPet>> GetGpsPetsPets(int id)
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer){
                    if (this.IsClient){
                        var query1 = _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking).Where(gpsPet => gpsPet.ClientId == CurrentClient.Id  && gpsPet.PetId == id).Select(gpsPet => new GpsPet {
                        Id = gpsPet.Id,
                        DeviceTraking = gpsPet.DeviceTraking,
                        DeviceTrakingId = gpsPet.DeviceTrakingId,
                        Status = gpsPet.Status,
                        });
                        return await query1.FirstOrDefaultAsync();
                    }
                    return Unauthorized();
                }
            return await _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking).Where(gpsPet => gpsPet.PetId == id).FirstOrDefaultAsync();
        }

        // GET: api/GpsPets/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GpsPet>> GetGpsPet(int id)
        {
            var gpsPet = await _context.GpsPet.FindAsync(id);

            if (gpsPet == null)
            {
                return NotFound();
            }

            return gpsPet;
        }

        // PUT: api/GpsPets/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutGpsPet(int id, GpsPet gpsPet)
        {
            if (id != gpsPet.Id)
            {
                return BadRequest();
            }

            _context.Entry(gpsPet).State = EntityState.Modified;

            try
            {
                var gpsPet2 = await _context.GpsPet.Where(gpsPet2=>(gpsPet2.PetId == gpsPet.PetId)).Select(gpsPet2 => new GpsPet {
                    Id = gpsPet2.Id,
                    PetId = gpsPet2.PetId,
                }).FirstOrDefaultAsync();

                if (gpsPet2 != null && !IsAdmin && !IsAuthorizer)
                {
                    throw new Exception("La mascota ya cuenta con un collar, comuniquese con los administradores.");
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GpsPetExists(id))
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

        // POST: api/GpsPets
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GpsPet>> PostGpsPet(GpsPet gpsPet)
        {
            _context.GpsPet.Add(gpsPet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGpsPet", new { id = gpsPet.Id }, gpsPet);
        }

        // DELETE: api/GpsPets/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<GpsPet>> DeleteGpsPet(int id)
        {
            var gpsPet = await _context.GpsPet.FindAsync(id);
            if (gpsPet == null)
            {
                return NotFound();
            }

            _context.GpsPet.Remove(gpsPet);
            await _context.SaveChangesAsync();

            return gpsPet;
        }
        private bool GpsPetExists(int id)
        {
            return _context.GpsPet.Any(e => e.Id == id);
        }
    }
}
