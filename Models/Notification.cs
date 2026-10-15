using System.Collections.Generic;
namespace PetsApi.Models
{
    public class Notification : BaseModel {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool All { get; set; }
        public Client Client { get; set; }
        public int? ClientId { get; set; }
        public bool Read { get; set; }
    }
}
