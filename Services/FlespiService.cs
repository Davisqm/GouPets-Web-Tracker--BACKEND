using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetsApi.Models.GPS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PetsApi.Services
{
    public class FlespiService : IFlespiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public FlespiService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        // ============================================================
        // OBTENER TODOS LOS DISPOSITIVOS DE FLESPI
        // ============================================================
        public async Task<string> GetDevicesAsync()
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"FlespiToken {_configuration["Flespi:Token"]}");

            var url = $"{_configuration["Flespi:BaseUrl"]}/devices/all";
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            return $"URL: {url}\nStatus: {(int)response.StatusCode}\n\n{content}";
        }

        // ============================================================
        // OBTENER POSICIÓN ACTUAL
        // ============================================================
        public async Task<GpsPosition> GetLastPositionAsync(int deviceId)
        {
            return await GetLastPositionByDeviceIdAsync(deviceId);
        }

        // ============================================================
        // OBTENER POSICIÓN ACTUAL POR DEVICE ID
        // ============================================================
        public async Task<GpsPosition> GetLastPositionByDeviceIdAsync(long deviceId)
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"FlespiToken {_configuration["Flespi:Token"]}");

            var url = $"{_configuration["Flespi:BaseUrl"]}/devices/{deviceId}/telemetry/position";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(json);
            var result = obj["result"];

            if (result == null || result.Type != JTokenType.Array || !result.Any())
            {
                return null;
            }

            var telemetry = result[0]["telemetry"];
            if (telemetry == null) return null;

            var position = telemetry["position"];
            if (position == null) return null;

            var value = position["value"];
            if (value == null) return null;

            double latitude = value["latitude"] != null ? value["latitude"].Value<double>() : 0;
            double longitude = value["longitude"] != null ? value["longitude"].Value<double>() : 0;
            double timestamp = position["ts"] != null ? position["ts"].Value<double>() : 0;
            bool valid = latitude != 0 && longitude != 0;

            return new GpsPosition
            {
                DeviceId = deviceId,
                DeviceName = "EV 201M",
                Imei = "867730056432143",
                Latitude = latitude,
                Longitude = longitude,
                Valid = valid,
                Timestamp = timestamp,
                BatteryLevel = 0
            };
        }

        // ============================================================
        // OBTENER POSICIÓN POR IMEI
        // ============================================================
        public async Task<dynamic> GetPositionByImeiAsync(string imei)
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"FlespiToken {_configuration["Flespi:Token"]}");

            var url = $"{_configuration["Flespi:BaseUrl"]}/gw/devices/{imei}/messages?data=%23position";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);

            if (obj == null || obj.result == null) return null;
            return obj.result;
        }

        // ============================================================
        // OBTENER HISTORIAL DE RECORRIDO POR IMEI
        // ============================================================
        public async Task<List<GpsHistoryPosition>> GetHistoryByImeiAsync(string imei, string from, string to)
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"FlespiToken {_configuration["Flespi:Token"]}");

            double fromTimestamp = ConvertDateToUnix(from);
            double toTimestamp = ConvertDateToUnix(to);

            string deviceSelector = $"configuration.ident=\"{imei}\"";

            var data = new
            {
                count = 10000,
                from = fromTimestamp,
                to = toTimestamp,
                fields = "timestamp,device.id,device.name,ident,position.latitude,position.longitude,position.valid,position.speed,position.direction,battery.level,battery.voltage,battery.charging.status"
            };

            string jsonData = JsonConvert.SerializeObject(data);
            string encodedData = Uri.EscapeDataString(jsonData);

            string url = $"{_configuration["Flespi:BaseUrl"]}/devices/{deviceSelector}/messages?data={encodedData}";

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(json);
            var result = obj["result"];

            var history = new List<GpsHistoryPosition>();
            if (result == null || result.Type != JTokenType.Array)
            {
                return history;
            }

            // Flespi solo incluye en cada mensaje los campos que llegaron en ese momento,
            // así que se arrastra el último valor conocido de batería.
            double? lastBatteryLevel = null;
            double? lastBatteryVoltage = null;
            bool? lastCharging = null;

            foreach (var message in result)
            {
                if (message["position.latitude"] == null || message["position.longitude"] == null)
                {
                    continue;
                }

                double latitude = message["position.latitude"].Value<double>();
                double longitude = message["position.longitude"].Value<double>();

                if (latitude == 0 && longitude == 0)
                {
                    continue;
                }

                double timestamp = message["timestamp"] != null ? message["timestamp"].Value<double>() : 0;
                long deviceId = message["device.id"] != null ? message["device.id"].Value<long>() : 0;
                string deviceName = message["device.name"] != null ? message["device.name"].Value<string>() : "EV 201M";
                string messageImei = message["ident"] != null ? message["ident"].Value<string>() : imei;
                bool valid = message["position.valid"] != null ? message["position.valid"].Value<bool>() : true;

                lastBatteryLevel = message["battery.level"]?.Value<double?>() ?? lastBatteryLevel;
                lastBatteryVoltage = message["battery.voltage"]?.Value<double?>() ?? lastBatteryVoltage;
                lastCharging = message["battery.charging.status"]?.Value<bool?>() ?? lastCharging;

                history.Add(new GpsHistoryPosition
                {
                    DeviceId = deviceId,
                    DeviceName = deviceName,
                    Imei = messageImei,
                    Latitude = latitude,
                    Longitude = longitude,
                    Valid = valid,
                    Timestamp = timestamp,
                    ServerTime = DateTimeOffset.FromUnixTimeMilliseconds((long)(timestamp * 1000)).UtcDateTime,
                    Address = null,
                    Attributes = new GpsHistoryAttributes
                    {
                        BatteryLevel = lastBatteryLevel,
                        BatteryVoltage = lastBatteryVoltage,
                        Charging = lastCharging,
                        Speed = message["position.speed"]?.Value<double?>(),
                        Direction = message["position.direction"]?.Value<double?>()
                    }
                });
            }

            return history;
        }

        // ============================================================
        // CONVERTIR FECHA A UNIX TIMESTAMP
        // ============================================================
        private double ConvertDateToUnix(string date)
        {
            if (string.IsNullOrWhiteSpace(date)) throw new ArgumentException("La fecha no puede estar vacía.");

            DateTime parsedDate;

            if (!DateTime.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsedDate))
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    throw new ArgumentException($"No se pudo interpretar la fecha: {date}");
                }
            }

            if (parsedDate.Kind == DateTimeKind.Unspecified)
            {
                var colombiaOffset = TimeSpan.FromHours(-5);
                var colombiaDate = new DateTimeOffset(parsedDate, colombiaOffset);
                return colombiaDate.ToUnixTimeMilliseconds() / 1000.0;
            }

            return new DateTimeOffset(parsedDate).ToUnixTimeMilliseconds() / 1000.0;
        }
    }
}