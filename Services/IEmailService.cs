using System.Collections.Generic;
using System.Threading.Tasks;
using PetsApi.Models;
using Microsoft.AspNetCore.Mvc;
namespace PetsApi.Services

//_ebb
{
   public interface IEmailService
    {
        Task SendEmailAsync(EmailInfo emailInfo);
        Task SendEmailTemplateAsync(EmailSource emailSource);
        Task<ActionResult<IEnumerable<GpsPet>>> GetGpsPetsClient2();
    }
}