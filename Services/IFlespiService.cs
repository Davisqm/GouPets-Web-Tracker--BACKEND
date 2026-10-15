using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PetsApi.Models.GPS;

namespace PetsApi.Services
{
    public interface IFlespiService
    {
        Task<string> GetDevicesAsync();

        Task<GpsPosition> GetLastPositionAsync(int deviceId);

        Task<dynamic> GetPositionByImeiAsync(string imei);

        Task<GpsPosition> GetLastPositionByDeviceIdAsync(long deviceId);

        Task<List<GpsHistoryPosition>> GetHistoryByImeiAsync(
            string imei,
            string from,
            string to);
    }
}