using PetsApi.Models;

namespace PetsApi.Models.Custom
{
    public class UserDto : User
    {
      public string Token { get; set; }
      public string SupplierName { get; set; }

      public string NewPassword { get; set; }
    }
}
