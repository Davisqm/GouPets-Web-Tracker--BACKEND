using System.Collections.Generic;
namespace PetsApi.Models
{
    public class Rate : BaseModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public Specie Specie { get; set; }
        public int SpecieId { get; set; }
    }
}