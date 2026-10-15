using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SammgpsApi.Model.Samm
{
  public class User
  {
    public long Id { get; set; }
    public string Names { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedDate { get; set; }
    public short UTC { get; set; }
    public int ClientId { get; set; }
    public bool Active { get; set; }
    public char? CSVSplitter { get; set; }
    public string Alias { get; set; }
    public DateTime? ExpireOn { get; set; }
    public string SignedPassword { get; set; }
  }
}
