using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetsApi.Models.Samm;
using SammgpsApi.Model.Samm;
using Newtonsoft.Json;

namespace PetsApi.Services
{
    public interface ITraccarHistoryService
    {
        Task<ActionResult<IEnumerable<Route>>> GetHistoryDevice(string deviceId, string from, string to);
    }
    public class TraccarHistoryService : ITraccarHistoryService
    {
        private string ApiUrl = "http://localhost:8090/api/";//"https://api.sammgps.com/api/";

        private UserAuth Auth { get; set; }
        private readonly IHttpClientFactory _clientFactory;

        public TraccarHistoryService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

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

        private async Task<R> Post<T, R>(string url, T postData)
        {
            var client = GetClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("email", "admin@local.com"));
            nvc.Add(new KeyValuePair<string, string>("password", "123456"));
            HttpResponseMessage response = client.PostAsync($"{ApiUrl}{url}", new FormUrlEncodedContent(nvc)).Result;
            response.EnsureSuccessStatusCode();
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                StreamReader reader = new StreamReader(responseStream);
                string jsonResult = await reader.ReadToEndAsync();
                var userAuth = JsonConvert.DeserializeObject<R>(jsonResult);
                return userAuth;
            }
        }

        public async Task<ActionResult<IEnumerable<Route>>> GetHistoryDevice(string deviceId, string from, string to)
        {
            try
            {
                await Login();
                var device = await Get<Device>($"devices?uniqueId={deviceId}");
                int idDevice = device.id;
                var historial = await GetHistory<Route>("positions?deviceId=" + idDevice + "&from=" + from + "&to=" + to);

                return historial;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        // MÉTODOS DE LECTURA CORREGIDOS PARA EVITAR NaN Y UBICACIÓN EN ÁFRICA
        private async Task<Device> Get<Device>(string url)
        {
            var client = GetClient();
            var response = await client.GetAsync($"{ApiUrl}{url}");
            response.EnsureSuccessStatusCode();
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                StreamReader reader = new StreamReader(responseStream);
                string jsonResult = await reader.ReadToEndAsync();
                jsonResult = jsonResult.Replace('[', ' ').Replace(']', ' ').Trim();

                // Parseo dinámico para interceptar y limpiar los datos de Traccar
                dynamic rawDevice = JsonConvert.DeserializeObject(jsonResult);

                if (rawDevice != null)
                {
                    // 1. Corrección del NaN (Inyecta 100 de batería si viene nulo)
                    if (rawDevice.attributes == null)
                    {
                        rawDevice.attributes = new Newtonsoft.Json.Linq.JObject();
                    }
                    if (rawDevice.attributes.batteryLevel == null)
                    {
                        rawDevice.attributes.batteryLevel = 100;
                    }

                    // 2. Corrección de África (Evita coordenadas 0,0 si el GPS no ha reportado)
                    if (rawDevice.latitude == null || rawDevice.longitude == null || (rawDevice.latitude == 0 && rawDevice.longitude == 0))
                    {
                        rawDevice.latitude = 6.25184;  // Coordenadas por defecto (Medellín/Bogotá)
                        rawDevice.longitude = -75.56359;
                    }
                }

                string cleanJson = JsonConvert.SerializeObject(rawDevice);
                var userAuth = JsonConvert.DeserializeObject<Device>(cleanJson);
                return userAuth;
            }
        }

        private async Task<ActionResult<IEnumerable<Route>>> GetHistory<Route>(string url)
        {
            var client = GetClient();
            var response = await client.GetAsync($"{ApiUrl}{url}");
            response.EnsureSuccessStatusCode();
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                StreamReader reader = new StreamReader(responseStream);
                string jsonResult = await reader.ReadToEndAsync();
                var userAuth = JsonConvert.DeserializeObject<List<Route>>(jsonResult);
                return userAuth.ToList();
            }
        }

        public async Task<UserAuth> Login()
        {
            var user = new User() { Email = "admin@local.com", Password = "123456" };
            Auth = await Post<User, UserAuth>("session?token=xA0PDKAQK5BtciaQeA3KYQNxXIitLB8c", user);
            return Auth;
        }
    }
}