using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;
using static PetsApi.Common.Enums;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : TMSController
    {
        private readonly SuppliersContext _context;

        public ClientsController(SuppliersContext context) :base(context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClient()
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer) 
                        return Unauthorized();
            return await _context.Client.Include(client=>client.User).ToListAsync();
        }

        // Obtengo los clientes que no han sido eliminados
        [HttpGet("get-clients-availables")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientsAvailables()
        {
            if (!this.IsAdmin) 
                if (!this.IsAuthorizer) 
                        return Unauthorized(); 
            return await _context.Client.Include(client=>client.User).Where(client=>client.Deleted == false).ToListAsync();
        }

        // Obtener los clientes eliminados
        [HttpGet("get-clients-deleted")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientsDeleted()
        {
            if (!this.IsAdmin)
                if (!this.IsAuthorizer) 
                        return Unauthorized();
            return await _context.Client.Include(client=>client.User).Where(client=>client.Deleted == true).ToListAsync();
        }

        // Obtengo la id de usuario del cliente loggueado y mando la informacion a la pagina de perfl para mostrarla y editarla
        [HttpGet("get-client-logged")]
        public async Task<ActionResult<Client>> GetClientLogged()
        {
            if(!IsAdmin){
                if(!IsAuthorizer){
                    return await _context.Client.Where(client=>client.UserId == CurrentClient.UserId).FirstOrDefaultAsync();
                }
            }
            //throw new Exception("Los Admins y Autorizados no tienen un perfil de cliente");
            return NoContent();

        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Client.Include(client => client.GpsPets).Where(client=>client.Id == id).Select(client => new Client {
                Id = client.Id,
                UserId = client.UserId,
                User = client.User,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Document = client.Document,
                Phone = client.Phone,
                Address = client.Address,
                Deleted = client.Deleted,
                Pets = client.Pets,
                GpsPets = client.GpsPets,
            }).FirstOrDefaultAsync();

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                var client2 = await _context.Client.Include(Client => Client.User).Where(client2=>((client2.Document == client.Document || client2.User.Email == client.User.Email) && client2.Id != client.Id)).Select(client2 => new Client {
                Id = client2.Id,
                Document = client2.Document,
                User = new User {
                    Email = client2.User.Email,
                },
                }).FirstOrDefaultAsync();

                if (client2 != null)
                { 
                    if (client2.Document == client.Document){
                        throw new Exception("Ya existe un usuario con este documento.");
                    }
                    else {
                        if ((client2.User.Email).Equals(client.User.Email)){
                            throw new Exception("Ya existe un usuario con este correo");
                        }
                        else {
                            throw new Exception("Por favor notificar como salio este error.");
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // Registro clientes y usuario
        // POST: api/Clients
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            var client2 = await _context.Client.Include(Client => Client.User).Where(client2=>(client2.Document == client.Document) || client2.User.Email == client.User.Email).Select(client2 => new Client {
                Id = client2.Id,
                Document = client2.Document,
                User = new User {
                    Email = client2.User.Email,
                },

            }).FirstOrDefaultAsync();

            if (client2 != null)
            {
                if (client2.Document == client.Document){
                    throw new Exception("Ya existe un usuario con este documento.");
                }
                else {
                    if ((client2.User.Email).Equals(client.User.Email)){
                        throw new Exception("Ya existe un usuario con este correo");
                    }
                    else {
                        throw new Exception("Por favor notificar como salio este error.");
                    }
                }
            }

            client.User.Name= $"{client.FirstName} {client.LastName}";
            client.User.RoleId= (int) UserRole.Client;
            client.User.Active= true;
            _context.Client.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Client.Remove(client);
            await _context.SaveChangesAsync();

            return client;
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.Id == id);
        }
    }
}
