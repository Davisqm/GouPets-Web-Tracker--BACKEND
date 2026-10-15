using System.Collections.Generic;
namespace PetsApi.Models
{
    public class TicketMessage : BaseModel {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsClient { get; set; }
        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }
    }
}
