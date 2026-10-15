using System;
namespace PetsApi.Models
{
    public class TrakingAccess : BaseModel {     

        public int Id { get; set; }
        public GpsPet GpsPet { get; set; }
        public int GpsPetId { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }

    }
}   
