namespace PetsApi.Models
{
    public class Vaccine : BaseModel {     

        public int Id { get; set; }
        public string Name {get; set; }
        public string Type {get; set; }
        public string Date {get; set; }
        public string Description {get; set; }
        public Pet Pet {get; set; }
        public int PetId {get; set; }         
    }
    
}   
