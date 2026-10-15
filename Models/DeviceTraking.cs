namespace PetsApi.Models
{
    public class DeviceTraking
    {
        public int Id { get; set; }

        public long? FlespiDeviceId { get; set; }

        public string Imei { get; set; }

        public bool Status { get; set; }

        public bool Condition { get; set; }

        public string BatteryLevel { get; set; }
    }
}