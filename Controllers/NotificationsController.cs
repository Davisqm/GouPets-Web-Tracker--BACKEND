using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;
using PetsApi.Models.Custom;
using PetsApi.Services;
using Microsoft.AspNetCore.Authorization;
using RestSharp;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : TMSController
    {
        private readonly SuppliersContext _context;
        //_ebb
        private readonly IEmailService _mailService;

        public NotificationsController(SuppliersContext context, IEmailService mailService) : base(context)
        {
            _context = context;
            _mailService = mailService;
        }

    

        // GET: api/Notifications
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotification()
        {
            return await _context.Notification.ToListAsync();
        }


        // GET: api/Notifications/Client Logged
        [HttpGet("get-notification-client")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationClient()
        {
            if(!IsAdmin &&  !IsAuthorizer && IsClient)
                return await _context.Notification.Where(notification => notification.Read == false && (notification.ClientId == CurrentClient.Id || notification.All == true)).ToListAsync();
            return new List<Notification>();
        }

        // GET: api/Notifications/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Notification>> GetNotification(int id)
        {
            var notification = await _context.Notification.FindAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            return notification;
        }

        // PUT: api/Notifications/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutNotification(int id, Notification notification)
        {
            if (id != notification.Id)
            {
                return BadRequest();
            }

            _context.Entry(notification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
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

        // POST: api/Notifications
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Notification>> PostNotification(NotificationDto notification)
        {
            if(notification.ClientIds != null){
                foreach (var clientId in notification.ClientIds){
                        _context.Notification.Add(new Notification(){
                        Title = notification.Title,
                        Date = notification.Date,
                        Type = notification.Type,
                        Description = notification.Description,
                        ClientId = clientId,
                    });                    
                }
                await _context.SaveChangesAsync();  
            }else {
             _context.Notification.Add(notification);
             await _context.SaveChangesAsync();          
            }            
            return CreatedAtAction("GetNotification", new { id = notification.Id }, notification);
        }


        [HttpPost("sendEmailTemplate")]
        [Authorize]
        public async Task<IActionResult> SendWelcomeMail(EmailSource source)
        {
            try
            {
               
                await _mailService.SendEmailTemplateAsync(source);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("RecoveryEmail")]
        public async Task<IActionResult> RecoveryEmail(EmailSource source)
        {
            try
            {
              var user = await _context.Users.Where(user => user.Email == source.EmailTo).FirstOrDefaultAsync();               
                if (user == null)
                {
                    return NotFound();
                }

                var password = user.Password;
                source.Password = password;                        
                await _mailService.SendEmailTemplateAsync(source);

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // DELETE: api/Notifications/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Notification>> DeleteNotification(int id)
        {
            var notification = await _context.Notification.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            _context.Notification.Remove(notification);
            await _context.SaveChangesAsync();

            return notification;
        }

        private bool NotificationExists(int id)
        {
            return _context.Notification.Any(e => e.Id == id);
        }
    }
}
