using Newtonsoft.Json;

namespace PetsApi.Models.GPS
{
    public class FlespiPosition
    {
        [JsonProperty("device.id")]
        public long DeviceId { get; set; }

        [JsonProperty("device.name")]
        public string DeviceName { get; set; }

        [JsonProperty("ident")]
        public string Ident { get; set; }

        [JsonProperty("position.latitude")]
        public double Latitude { get; set; }

        [JsonProperty("position.longitude")]
        public double Longitude { get; set; }

        [JsonProperty("position.valid")]
        public bool Valid { get; set; }

        [JsonProperty("timestamp")]
        public double Timestamp { get; set; }

        // Agrega esta propiedad para solucionar el error de compilación y el NaN en la tabla
        [JsonProperty("battery.level")]
        public double BatteryLevel { get; set; }
    }
}