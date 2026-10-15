using System;

namespace PetsApi.Models.Samm
{
  public class Device
  {
    public int id { get; set; }
    public string? name { get; set; }
    public string? uniqueId { get; set; }
    public string? status { get; set; }
    public bool? disabled { get; set; }
    public string? lastUpdate { get; set; }
    public int positionId { get; set; }
    public int? groupId { get; set; }
    public string? phone { get; set; }
    public string? model { get; set; }
    public string? contact { get; set; }
    public string? category { get; set; }
  }
}
