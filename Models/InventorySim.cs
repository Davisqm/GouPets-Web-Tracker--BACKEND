using System.Collections.Generic;
namespace PetsApi.Models
{
    public class InventorySim : BaseModel {
        public int Id { get; set; }
        public bool Status { get; set; }
        public bool Condition { get; set; }
        public string CellOperator { get; set; }
        public string LineNumber { get; set; }
        public string DateCutOff { get; set; }
    }
}
