using System.Collections.Generic;
namespace PetsApi.Models
{
    public class GpsPet : BaseModel {
        public int Id { get; set; }
        public Client Client { get; set; }
        public int? ClientId { get; set; }
        public Pet Pet { get; set; }
        public int? PetId { get; set; }
        public virtual DeviceTraking DeviceTraking { get; set; }
        public int DeviceTrakingId { get; set; }
        public virtual InventorySim InventorySim { get; set; }
        public int InventorySimId { get; set; }
        public bool Status { get; set; }
    }
}
