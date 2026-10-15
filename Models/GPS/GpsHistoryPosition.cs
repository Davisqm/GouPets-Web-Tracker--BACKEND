using System;

namespace PetsApi.Models.GPS
{
    public class GpsHistoryPosition
    {
        public long DeviceId { get; set; }

        public string DeviceName { get; set; }

        public string Imei { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool Valid { get; set; }

        public double Timestamp { get; set; }

        public DateTime ServerTime { get; set; }

        public string Address { get; set; }

        public GpsHistoryAttributes Attributes { get; set; }
    }

    public class GpsHistoryAttributes
    {
        public double? BatteryLevel { get; set; }

        public double? BatteryVoltage { get; set; }

        public bool? Charging { get; set; }

        public double? Speed { get; set; }

        public double? Direction { get; set; }
    }
}