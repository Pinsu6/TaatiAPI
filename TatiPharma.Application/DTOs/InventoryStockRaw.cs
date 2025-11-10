using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class InventoryStockRaw
    {
        //public long DrugId { get; set; }
        //public string Name { get; set; } = string.Empty;
        //public string Category { get; set; } = string.Empty;
        //public long RemainStock { get; set; }
        //public int? MinLevel { get; set; }
        //public int? MaxLevel { get; set; }
        //public decimal UnitCost { get; set; }

        public long? DrugId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public long RemainStock { get; set; }
        public int? MinLevel { get; set; }
        public int? MaxLevel { get; set; }
        public decimal UnitCost { get; set; }
    }
}
