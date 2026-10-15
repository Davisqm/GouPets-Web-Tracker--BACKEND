
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PetsApi.ViewModels;
using PetsApi.Models;
using static PetsApi.Common.Enums;

namespace PetsApi.Services
{
  public interface INotifyService
  {
    Task SendDocumemtsRequested(DocumentsRequestedViewModel model);
  }
  public class NotifyService : INotifyService
  {
    private readonly IRazorViewToStringRenderer _renderer;
    private readonly IUsersService _usersService;
    private readonly SuppliersContext _context;
    private readonly IConfiguration _configuration;

    private string EmailSmtpDomain;
    private int EmailSmtpPort;
    private string EmailSmtpUserName;
    private string EmailSmtpPassword;
    private string EmailFromAddress;
    private string EmailFromName;
    private bool EmailSmtpUseSsl;
    private string WebUrl;

    public NotifyService(IRazorViewToStringRenderer renderer, IUsersService usersService, SuppliersContext context, IConfiguration configuration)
    {
      _renderer = renderer;
      _usersService = usersService;
      _context = context;
      _configuration = configuration;
      EmailSmtpDomain = _configuration.GetValue<string>("EmailSmtpDomain");
      EmailSmtpPort = _configuration.GetValue<int>("EmailSmtpPort");
      EmailSmtpUserName = _configuration.GetValue<string>("EmailSmtpUserName");
      EmailSmtpPassword = _configuration.GetValue<string>("EmailSmtpPassword");
      EmailFromAddress = _configuration.GetValue<string>("EmailFromAddress");
      EmailFromName = _configuration.GetValue<string>("EmailFromName");
      EmailSmtpUseSsl = _configuration.GetValue<bool>("EmailSmtpUseSsl");
      WebUrl = _configuration.GetValue<string>("WebUrl");
    }

    public async Task SendDocumemtsRequested(DocumentsRequestedViewModel model)
    {
      model.WebUrl = WebUrl;
      var supplierUsers = await _usersService.GetByRol(UserRole.Supplier);
      // await CreateNotification(NotificationType.CarrierRegistered, "New carrier user registered", managers);
      // await this.SendEmail("Please attach documents", "DocumentsRequested", model, supplierUsers);
    }


    public async Task SendEmail<TModel>(string subject, string viewName, TModel model, User destination) {
      var destinationList = new List<User>() { destination };
      await SendEmail(subject, viewName, model, destinationList);
    }
    public async Task SendEmail<TModel>(string subject, string viewName, TModel model, List<User> destination)
    {
      try {
        var from = new MailAddress(EmailFromAddress, EmailFromName);
        var to = new MailAddressCollection();
       

        string view = $"/Views/Emails/{viewName}";
        var htmlBody = await _renderer.RenderViewToStringAsync($"{view}.cshtml", model);

        var message = new MailMessage() {
          From = from,
          Subject = subject,
          Body = htmlBody,
          BodyEncoding = Encoding.UTF8,
          IsBodyHtml = true
        };
        foreach (var user in destination)
          message.To.Add(user.Email);

        using (var smtp = new SmtpClient(EmailSmtpDomain, EmailSmtpPort)) {
          smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
          smtp.UseDefaultCredentials = false;
          smtp.EnableSsl = EmailSmtpUseSsl;
          smtp.Credentials = new NetworkCredential(EmailSmtpUserName, EmailSmtpPassword);
          await smtp.SendMailAsync(message);
        }
      } catch (Exception e) {
        throw new Exception($"Failed to send email: {e.Message}", e);
      }
    }
  }
}
