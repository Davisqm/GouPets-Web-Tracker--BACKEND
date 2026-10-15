using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetsApi.Services;
using PetsApi.Models;
using PetsApi.Models.Custom;
using static PetsApi.Common.Enums;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : TMSController
    {
        private readonly IConfiguration _configuration;
        private readonly SuppliersContext _context;
        private readonly IUsersService _usersService;
        private readonly INotifyService _notifier;

        public UsersController(SuppliersContext context, IConfiguration configuration, IUsersService usersService, INotifyService notifier): base(context)
        {
            _context = context;
            _configuration = configuration;
            _usersService = usersService;
            _notifier = notifier;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery]bool? ignoreClients)
        {
            // if (!this.IsAdmin && !this.IsAuthorizer)
            if (!this.IsAdmin)
                if(!this.IsAuthorizer)
                    return Unauthorized();
            var quey = _context.Users.Where(user => user.RoleId != 0);
            
            if (ignoreClients == true) {
                var users2 = await quey.Where(user=>user.RoleId != (int) UserRole.Client && user.RoleId != (int) UserRole.Admin).ToListAsync();
                return users2.Select(user => _usersService.MapUser(user)).ToList();
            }

            var users = await quey.ToListAsync();
            return users.Select(user => _usersService.MapUser(user)).ToList();

        }







        // Obtengo la id de usuario del cliente loggueado y mando la informacion a la pagina de perfl para mostrarla y editarla
        [HttpGet("get-user-logged")]
        [Authorize]
        public async Task<ActionResult<User>> GetUserLogged()
        {
              return await _context.Users.Where(user=>user.Id == CurrentUser.Id).FirstOrDefaultAsync();              
            
            
        }








        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return _usersService.MapUser(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }


            // if (!this.IsAdmin)
            //     if (!this.IsAuthorizer) 
            //         return Unauthorized();

            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (dbUser == null) throw new Exception("User doesn't exist");

            
            var user2 = await _context.Users.Where(user2=>(user2.Email == user.Email && user2.Id != user.Id)).Select(user2 => new User {
                Id = user2.Id,
                Email = user2.Email,
            }).FirstOrDefaultAsync();

            if (user2 != null)
            {
                throw new Exception("Correo asociado con otro usuario");
            }

            dbUser.Name = user.Name;
            dbUser.Active = user.Active;
            dbUser.Email = user.Email;
            
            if (this.CurrentUser.RoleId == (int)UserRole.Admin) {
                dbUser.RoleId = user.RoleId;
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> PostUser(UserDto user)
        {
            var user2 = await _context.Users.Where(user2=>(user2.Email == user.Email && user2.Id != user.Id)).Select(user2 => new User {
                Id = user2.Id,
                Email = user2.Email,
            }).FirstOrDefaultAsync();

            if (user2 != null)
            {
                throw new Exception("Correo asociado con otro usuario");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new User() { Id = user.Id };
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<User>> ChangePassword(UserDto user)
        {
            if (this.CurrentUser.Password != user.Password) {
                throw new Exception("BackMessage auth.invalidCurrentPassword");
            }

            var dbUser = await _context.Users.FindAsync(this.CurrentUser.Id);
            if (dbUser == null) {
                throw new Exception("User doesn't exist");
            }
            if (string.IsNullOrEmpty(user.NewPassword)) {
                throw new Exception("BackMessage auth.passwordCannotBeEmpty");
            }
            dbUser.Password = user.NewPassword;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
