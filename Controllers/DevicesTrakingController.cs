using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesTrakingController : TMSController
    {
        private readonly SuppliersContext _context;

        public DevicesTrakingController(SuppliersContext context) :base(context)
        {
            _context = context;
        }

        // GET: api/DevicesTraking
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<DeviceTraking>>> GetDeviceTraking()
        {
            return await _context.DeviceTraking.ToListAsync();
        }

        // GET: api/DevicesTraking/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<DeviceTraking>> GetDeviceTraking(int id)
        {
            var deviceTraking = await _context.DeviceTraking.FindAsync(id);

            if (deviceTraking == null)
            {
                return NotFound();
            }

            return deviceTraking;
        }

        [HttpGet("get-device-traking-available")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<DeviceTraking>>> GetDeviceTrakingAvailable()
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer) 
                    return Unauthorized();
            return await _context.DeviceTraking.Where(devicetraking=> devicetraking.Status == false && devicetraking.Condition == true).ToListAsync();
        }

        // PUT: api/DevicesTraking/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutDeviceTraking(int id, DeviceTraking deviceTraking)
        {
            if (id != deviceTraking.Id)
            {
                return BadRequest();
            }

            _context.Entry(deviceTraking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceTrakingExists(id))
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

        // POST: api/DevicesTraking
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<DeviceTraking>> PostDeviceTraking(DeviceTraking deviceTraking)
        {
            var deviceTraking2 = await _context.DeviceTraking.Where(deviceTraking2=>(deviceTraking2.Imei == deviceTraking.Imei)).Select(deviceTraking2 => new DeviceTraking {
                Id = deviceTraking2.Id,
                Imei = deviceTraking2.Imei,
            }).FirstOrDefaultAsync();

            if (deviceTraking2 != null)
            {
                throw new Exception("Ya existe");
            }


            try {
                var traccarDevicesUrl = "http://localhost:8090/api/devices";
                var traccarAuth = "Basic YWRtaW5AbG9jYWwuY29tOjEyMzQ1Ng==";
                var lookupRequest = (HttpWebRequest)WebRequest.Create($"{traccarDevicesUrl}?uniqueId={WebUtility.UrlEncode(deviceTraking.Imei)}");
                lookupRequest.Method = "GET";
                lookupRequest.Headers["Authorization"] = traccarAuth;

                var existsInTraccar = false;
                using (var lookupResponse = (HttpWebResponse)lookupRequest.GetResponse())
                using (var streamReader = new StreamReader(lookupResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    existsInTraccar = !string.IsNullOrWhiteSpace(result) && result.Trim() != "[]";
                }

                if (!existsInTraccar) {
                    var httpRequest = (HttpWebRequest)WebRequest.Create(traccarDevicesUrl);
                    httpRequest.Method = "POST";
                    httpRequest.Headers["Authorization"] = traccarAuth;
                    httpRequest.ContentType = "application/json";
                    var data = JsonConvert.SerializeObject(new {
                        name = deviceTraking.Imei,
                        uniqueId = deviceTraking.Imei,
                        phone = "",
                        model = "",
                        contact = "",
                        category = (string)null,
                        groupId = 0,
                        disabled = false
                    });
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                    }
                    using (var httpResponse = (HttpWebResponse)httpRequest.GetResponse())
                    {
                        Console.WriteLine(httpResponse.StatusCode);
                    }
                }

                _context.DeviceTraking.Add(deviceTraking);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDeviceTraking", new { id = deviceTraking.Id }, deviceTraking);

            } catch(Exception ex) {
                throw new Exception(ex.ToString());
            }
            
        }

        // DELETE: api/DevicesTraking/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<DeviceTraking>> DeleteDeviceTraking(int id)
        {
            var deviceTraking = await _context.DeviceTraking.FindAsync(id);
            if (deviceTraking == null)
            {
                return NotFound();
            }

            _context.DeviceTraking.Remove(deviceTraking);
            await _context.SaveChangesAsync();

            return deviceTraking;
        }

        private bool DeviceTrakingExists(int id)
        {
            return _context.DeviceTraking.Any(e => e.Id == id);
        }
    }
}
