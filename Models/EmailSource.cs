using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
//_ebb
namespace PetsApi.Models
{
    public class EmailSource
    {
      
        public string Logo { get; set; }
        public string ImgpPal { get; set; }
        public string IconoMensaje { get; set; }
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public string TextTitle { get; set; }
        public string TextMessage { get; set; }
        public string ImgBanner { get; set; }
        public string LinkBanner { get; set; }
        public string Fondofooter { get; set; }
        public string Issue { get; set; }
        public string Template { get; set; }
        public string Imgfacebook { get; set; }
        public string Imginstagram { get; set; }
        public string Imgwhatsapp { get; set; }
        //
        public string EmailTo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
