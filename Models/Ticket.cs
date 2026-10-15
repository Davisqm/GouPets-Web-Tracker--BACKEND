using System.Collections.Generic;
namespace PetsApi.Models
{
    public class Ticket : BaseModel {
        public int Id { get; set; }
        public string Type { get; set; } //Soporte, Bug, Quejas y reclamos
        public string Issue { get; set; } //Asunto
        public string Priority { get; set; }
        public bool Status { get; set; } //Pendiente o Resuelta
        public Client Client { get; set; }
        public int ClientId { get; set; }
        public List<TicketMessage> TicketMessages { get; set; }
    }
}
