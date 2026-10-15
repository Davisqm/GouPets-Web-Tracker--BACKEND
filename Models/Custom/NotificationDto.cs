using System.Collections.Generic;
using PetsApi.Models;

namespace PetsApi.Models.Custom
{
    public class NotificationDto : Notification
    {
      public List<int> ClientIds { get; set; }      
    }
}
