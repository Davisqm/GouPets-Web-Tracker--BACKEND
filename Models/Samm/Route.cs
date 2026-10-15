using System;

namespace PetsApi.Models.Samm
{
  public class Route
  {
    public string id { get; set; }
    public int? deviceId { get; set; }
    public string? protocol { get; set; }
    public DateTime? deviceTime{ get; set; }
    public DateTime? fixTime{ get; set; }
    public DateTime? serverTime{ get; set; }
    public bool outdated { get; set; }
    public bool valid { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public double? altitude { get; set; }
    public double? speed { get; set; }
    public double? course { get; set; }
    public string? address { get; set; }
    public double? accuracy { get; set; }
    public Attributes attributes { get; set; }

  }
}
