using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
//_ebb
namespace PetsApi.Models
{
    public class EmailInfo
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        //public List<IFormFile> Attachments { get; set; }
    }
}
