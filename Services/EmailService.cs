using PetsApi.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using PetsApi.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.IO;
using System;
using RestSharp;

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models.Samm;
using Microsoft.Extensions.DependencyInjection;


namespace PetsApi.Services
//_ebb
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _mailSettings;
        private readonly SuppliersContext _context;
        private readonly ISammService _sammService;
        public EmailService(IOptions<EmailSettings> mailSettings, IServiceScopeFactory factory,  ISammService sammService)
        {
            _mailSettings = mailSettings.Value;
            _context = factory.CreateScope().ServiceProvider.GetRequiredService<SuppliersContext>();
            _sammService = sammService;
            
        }
        public async Task SendEmailAsync(EmailInfo emailInfo)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.EMail);
            email.To.Add(MailboxAddress.Parse(emailInfo.EmailTo));
            email.Subject = emailInfo.Subject;
            var builder = new BodyBuilder();
            /*if (emailInfo.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in emailInfo.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }*/
            builder.HtmlBody = emailInfo.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.EMail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailTemplateAsync(EmailSource emailSource)        
        {


            /*var client = new RestClient("https://api.ultramsg.com/instance4868/messages/chat");
            var request = new RestRequest("",Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "token=3kdufebzj5704kje&to=+573005692011&body=La API de WhatsApp en UltraMsg.com funciona bien&priority=1&referenceId=", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);*/


            var clientR = new RestClient("https://service.testing.com/Api/Growth/EncodeDecodeString");
            var request = new RestRequest();
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "token=3kdufebzj5704kje&to=+573005692011&body=La API de WhatsApp en     UltraMsg.com funciona bien&priority=1&referenceId=", ParameterType.RequestBody);
            request.Method = Method.Post;
            var response1 = clientR.ExecutePostAsync(request);
            var content = response1.Result;
            var content1 = response1;

            /*string logo = "http://52.73.59.250:8084/img/goupets-logo.e295a9da.png";   
            string imgppal = "https://img.freepik.com/foto-gratis/adorable-perro-basenji-marron-blanco-sonriendo-dando-maximo-cinco-aislado-blanco_346278-1657.jpg?t=st=1646748301~exp=1646748901~hmac=ed3be2c21911599e31318e0695fc18a062a7c45743efe6100b1ca27471fdcc26&w=996";   
            string iconoMensaje = "https://www.clipartmax.com/png/full/183-1836317_electrical-cord-clip-art.png";   
            string titulo = emailSource.UserName + " " + emailSource.Password;
            string subtitulo = "Bateria baja";
            string TextTitle = "Explore our trending Category";
            string TextMessage = " ";
            string imgBanner = "http://grapestheme.com/notify/img//other/offers.jpg";
            string linkBanner = "http://grapestheme.com/notify/img//other/offers.jpg";*/
                   
          ///////////////////////////////////////////////////
           //string path = @"C:\Folder1\Folder2\Folder3\Folder4";
            string path = Directory.GetCurrentDirectory();
            //string FilePath = Path.GetFullPath(Path.Combine(path, @"..\\"+ templates,  ".html"));  
            string FilePath = Path.GetFullPath(Path.Combine(path, @"..\\"+emailSource.Template+"\\templates\\",  "Email.html"));  
            /*if (!Directory.Exists(FilePath))
            { }*/
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            //MailText = MailText.Replace("[username]", emailSource.UserName).Replace("[email]", emailSource.EmailTo);

            MailText = MailText.Replace("[logo]", emailSource.Logo)
                               .Replace("[imgppal]", emailSource.ImgpPal)
                               .Replace("[imgfacebook]", emailSource.Imgfacebook)
                               .Replace("[imginstagram]", emailSource.Imginstagram)
                               .Replace("[imgwhatsapp]", emailSource.Imgwhatsapp)
                               .Replace("[titulo]", emailSource.Titulo)
                               .Replace("[username]", emailSource.UserName)
                               .Replace("[mensaje]", emailSource.SubTitulo)
                               .Replace("[password]", emailSource.Password)
                               .Replace("[iconoMensaje]", emailSource.IconoMensaje)
                               .Replace("[TextTitle]", emailSource.TextTitle)
                               .Replace("[TextMessage]", emailSource.TextMessage + " "+ emailSource.Password)
                               .Replace("[imgBanner]", emailSource.ImgBanner)
                               .Replace("[fondofooter]", emailSource.Fondofooter)
                               .Replace("[linkBanner]", emailSource.LinkBanner);
            //
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.EMail);
            //email.To.Add(MailboxAddress.Parse(emailSource.EmailTo));
            email.To.Add(MailboxAddress.Parse(emailSource.EmailTo));
            email.To.Add(MailboxAddress.Parse("goupets1234@gmail.com"));
            //email.Subject = $"Welcome {emailSource.UserName}";
            email.Subject = $"{emailSource.Issue}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s,c,h,e) => true;
            smtp.Connect(_mailSettings.Host, _mailSettings.Port,  SecureSocketOptions.Auto);
            smtp.Authenticate(_mailSettings.EMail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public async Task<ActionResult<IEnumerable<GpsPet>>> GetGpsPetsClient2()
        {
            var query = _context.GpsPet.Include(gpsPet => gpsPet.DeviceTraking)
            .Include(gpsPet => gpsPet.Client)
            .ThenInclude(client => client.User)
            .Select(gpsPet => new GpsPet {
                Id = gpsPet.Id,
                ClientId = gpsPet.ClientId,
                Client = gpsPet.Client,
                DeviceTraking = gpsPet.DeviceTraking,
                DeviceTrakingId = gpsPet.DeviceTrakingId,
                Status = gpsPet.Status,
                });
                
            var emailSource = new EmailSource();
            foreach (GpsPet dato in query){
                Position position = await _sammService.GetDeviceById(dato.DeviceTraking.Imei.ToString());
                string batteryString;
                if (position != null) {
                    batteryString = position.attributes.batteryLevel;
                    if (position.attributes.batteryLevel != null && !(position.attributes.batteryLevel.Equals(""))){
                        int startIndex = 0;
                        int length = 2;
                        string substring = batteryString.Substring(startIndex, length);
                        int batteryLevel = int.Parse(substring);
                        if (batteryLevel <= 50){
                            emailSource.Logo = "http://app.goupets.com.co/templates/resources/goupets-logo-largo.png";   
                            emailSource.ImgpPal = "https://img.freepik.com/foto-gratis/adorable-perro-basenji-marron-blanco-sonriendo-dando-maximo-cinco-aislado-blanco_346278-1657.jpg?t=st=1646748301~exp=1646748901~hmac=ed3be2c21911599e31318e0695fc18a062a7c45743efe6100b1ca27471fdcc26&w=996";   
                            emailSource.Imgfacebook = "http://app.goupets.com.co/templates/resources/facebookHtml.png";   
                            emailSource.Imginstagram = "http://app.goupets.com.co/templates/resources/instagramHtml.png";   
                            emailSource.Imgwhatsapp = "http://app.goupets.com.co/templates/resources/whatsappHtml.png";   
                            emailSource.IconoMensaje = "https://www.clipartmax.com/png/full/183-1836317_electrical-cord-clip-art.png";   
                            emailSource.Titulo = "Alerta de Notificacion!";
                            emailSource.TextTitle = "Bateria baja";
                            emailSource.TextMessage = $"{dato.Client.User.Name}, Su bateria se encuentra en :{substring}%, te invitamos a recargar tu collar.";
                            emailSource.ImgBanner = "http://app.goupets.com.co/templates/resources/bannerEmailFooter.jpg";
                            emailSource.LinkBanner = "http://app.goupets.com.co/templates/resources/bannerEmailFooter.jpg";
                            emailSource.Fondofooter = "http://app.goupets.com.co/templates/resources/fondo1.png";
                            emailSource.EmailTo = "yamid.ninja10@gmail.com";//dato.Client.User.Email;
                            emailSource.Issue = $"Bateria baja '{dato.Client.User.Name}'";
                            emailSource.Template = "FrontGoupets";
                            await SendEmailTemplateAsync(emailSource);
                        }
                    }
                }
            }
            return null;
        }
    }
}
