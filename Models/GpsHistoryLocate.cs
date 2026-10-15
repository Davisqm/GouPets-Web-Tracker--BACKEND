using System.Collections.Generic;
namespace PetsApi.Models
{
    public class GpsHistoryLocate : BaseModel {
        public int Id { get; set; }       
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public GpsPet GpsPet { get; set;}
        public int GpsPetId { get; set;}
    }
}
