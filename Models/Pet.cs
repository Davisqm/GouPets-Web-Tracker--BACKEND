using System.Collections.Generic;

namespace PetsApi.Models
{
    public class Pet : BaseModel
    {
        public int Id { get; set; }

        public Client Client { get; set; }
        public int ClientId { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string Colour { get; set; }
        public string Fur { get; set; }
        public string Observations { get; set; }
        public string Photo { get; set; }
        public string Document { get; set; }
        public string Weight { get; set; }

        public Specie Specie { get; set; }
        public int SpecieId { get; set; }

        public int RateId { get; set; }

        public string Neutered { get; set; }
        public string Height { get; set; }

        // Información del GPS
        public string GpsProvider { get; set; }

        public long? GpsDeviceId { get; set; }

        public string GpsImei { get; set; }

        public bool GpsEnabled { get; set; }

        public List<Vaccine> Vaccines { get; set; }
        public List<Antiparasite> Antiparasites { get; set; }
    }
}