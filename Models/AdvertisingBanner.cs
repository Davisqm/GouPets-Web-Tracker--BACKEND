using System.Collections.Generic;
namespace PetsApi.Models
{
    public class AdvertisingBanner : BaseModel {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Image { get; set; }      
        public string Link { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string CutoffDate { get; set; }
      
    }
}
