namespace PetsApi.Models.GPS
{
    public class GpsPosition
    {
        public long DeviceId { get; set; }

        public string DeviceName { get; set; }

        public string Imei { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool Valid { get; set; }

        public double Timestamp { get; set; }

        // Agrega esta línea para que coincida con el servicio y elimine el NaN de la tabla
        public double BatteryLevel { get; set; }
    }
}