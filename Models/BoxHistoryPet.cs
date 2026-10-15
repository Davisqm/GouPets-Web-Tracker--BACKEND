namespace PetsApi.Models
{
    public class BoxHistoryPet : BaseModel {     

        public int Id { get; set; }
        public int petId { get; set;}
        public GpsPet gpsPet { get; set;}
        public int GpsPetId { get; set;}
        public string InitialDate { get; set; }
        public string FinalDate { get; set; }
    }
}
