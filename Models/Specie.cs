using System.Collections.Generic;
namespace PetsApi.Models
{
    public class Specie : BaseModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Rate> Rate { get; set; }
    }
}