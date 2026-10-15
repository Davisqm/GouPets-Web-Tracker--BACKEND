using System.Collections.Generic;
namespace PetsApi.Models
{
    public class Client : BaseModel {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Document { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool Deleted { get; set; }
        public List<Pet> Pets { get; set; }
        public List<GpsPet> GpsPets { get; set; }
    }
}   
