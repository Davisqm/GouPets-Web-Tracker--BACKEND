using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PetsApi.Models.Samm;
using SammgpsApi.Model.Samm;

namespace PetsApi.Services
{
    public interface ISammService
    {
        Task<Position> GetDeviceById(string deviceId);
        Task<string> ObtenerLinkMapa(string deviceId);
    }

    public class SammService : ISammService
    {
        // Configuración de URLs de tu servidor Traccar local
        private readonly string ApiUrl = "http://localhost:8090/api/";
        private readonly string TraccarWebUrl = "http://localhost:82";

        private UserAuth Auth { get; set; }
        private readonly IHttpClientFactory _clientFactory;

        public SammService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Genera el cliente HTTP insertando las cabeceras de autorización necesarias.
        /// </summary>
        private HttpClient GetClient()
        {
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("authorization", "Basic YWRtaW5AbG9jYWwuY29tOjEyMzQ1Ng==");

            if (Auth != null)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", $"Bearer {Auth.Token}");
            }
            return client;
        }

        /// <summary>
        /// Realiza peticiones POST codificadas en formulario hacia la API de Traccar.
        /// </summary>
        private async Task<R> Post<T, R>(string url, T postData)
        {
            var client = GetClient();
            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("email", "admin@local.com"),
                new KeyValuePair<string, string>("password", "123456")
            };

            HttpResponseMessage response = await client.PostAsync($"{ApiUrl}{url}", new FormUrlEncodedContent(nvc));
            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(responseStream))
            {
                string jsonResult = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<R>(jsonResult);
            }
        }

        /// <summary>
        /// Obtiene un dispositivo mediante su identificador único (IMEI) de Traccar.
        /// </summary>
        private async Task<Device> GetDevice(string url)
        {
            var client = GetClient();
            var response = await client.GetAsync($"{ApiUrl}{url}");
            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(responseStream))
            {
                string jsonResult = await reader.ReadToEndAsync();
                // Limpia los corchetes si la API devuelve un arreglo de un solo elemento
                jsonResult = jsonResult.Replace('[', ' ').Replace(']', ' ').Trim();
                return JsonConvert.DeserializeObject<Device>(jsonResult);
            }
        }

        /// <summary>
        /// Obtiene y normaliza la última posición registrada de un dispositivo, resolviendo el nivel de batería.
        /// </summary>
        private async Task<Position> GetPosition(string url)
        {
            var client = GetClient();
            var response = await client.GetAsync($"{ApiUrl}{url}");
            response.EnsureSuccessStatusCode();

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(responseStream))
            {
                string jsonResult = await reader.ReadToEndAsync();
                jsonResult = jsonResult.Replace('[', ' ').Replace(']', ' ').Trim();

                dynamic rawPosition = JsonConvert.DeserializeObject(jsonResult);

                if (rawPosition != null)
                {
                    if (rawPosition.attributes == null)
                    {
                        rawPosition.attributes = new Newtonsoft.Json.Linq.JObject();
                    }

                    // Mapeo dinámico de atributos de batería según el protocolo del GPS
                    if (rawPosition.attributes.batteryLevel != null)
                    {
                        // Parámetro nativo estándar de Traccar
                    }
                    else if (rawPosition.attributes.battery != null)
                    {
                        rawPosition.attributes.batteryLevel = rawPosition.attributes.battery;
                    }
                    else if (rawPosition.attributes.level != null)
                    {
                        rawPosition.attributes.batteryLevel = rawPosition.attributes.level;
                    }
                    else
                    {
                        // Solución al NaN: Forzamos un valor por defecto si no reporta batería
                        rawPosition.attributes.batteryLevel = 100;
                    }

                    // COORDENADAS POR DEFECTO: Si reporta en cero, centramos en La Unión, Antioquia
                    if (rawPosition.latitude == 0 && rawPosition.longitude == 0)
                    {
                        rawPosition.latitude = 5.9743;
                        rawPosition.longitude = -75.3622;
                    }
                }

                string cleanJson = JsonConvert.SerializeObject(rawPosition);
                return JsonConvert.DeserializeObject<Position>(cleanJson);
            }
        }

        /// <summary>
        /// Expone la posición de la mascota/dispositivo consultando Traccar mediante su IMEI.
        /// </summary>
        public async Task<Position> GetDeviceById(string deviceId)
        {
            try
            {
                await Login();
                var device = await GetDevice($"devices?uniqueId={deviceId}");
                if (device == null) return null;

                var position = await GetPosition($"positions?id={device.positionId}");

                if (position != null)
                {
                    // COORDENADAS POR DEFECTO: Si reporta en cero, centramos en La Unión, Antioquia
                    if (position.latitude == 0 && position.longitude == 0)
                    {
                        position.latitude = 5.9743;
                        position.longitude = -75.3622;
                    }
                }

                return position;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Genera el enlace compartido de Traccar para visualizar el mapa en tiempo real.
        /// </summary>
        public async Task<string> ObtenerLinkMapa(string deviceId)
        {
            try
            {
                await Login();
                var device = await GetDevice($"devices?uniqueId={deviceId}");
                if (device == null) return null;

                return $"{TraccarWebUrl}/shared/?token={Auth.Token}&deviceId={device.id}";
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Inicializa la sesión con el servidor de Traccar utilizando el Token activo proporcionado.
        /// </summary>
        public async Task<UserAuth> Login()
        {
            var user = new User { Email = "admin@local.com", Password = "123456" };

            // Se inyecta tu token de Traccar actual en la URL de sesión
            string tokenUrl = "session?token=RzBFAiA91sUTLsr5Xvw_y_uJhszBYHTiiO_SdjtL6gLt5pV1vAIhAOXXLiLTa5A-EMSyj6i-uFnZnKhN1HqE05W0RfiJAbVMeyJ1IjozNCwiZSI6IjIwMjYtMDctMDNUMTU6MjY6MTkuNjQzKzAwOjAwIn0";

            Auth = await Post<User, UserAuth>(tokenUrl, user);
            return Auth;
        }
    }
}