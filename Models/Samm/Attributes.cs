namespace PetsApi.Models.Samm
{
    public class Attributes
    {
        public string batteryLevel { get; set; }

        public string battery { get; set; }

        public double? totalDistance { get; set; }

        public double? distance { get; set; }

        public bool? ignition { get; set; }

        public int? satellites { get; set; }

        public double? hdop { get; set; }

        public string motion { get; set; }
    }
}
