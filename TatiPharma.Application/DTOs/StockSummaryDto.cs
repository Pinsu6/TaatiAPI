using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class StockSummaryDto
    {
        public long TotalPurchased { get; set; }
        public long TotalSold { get; set; }
        public long CurrentStock { get; set; } // Computed: purchased - sold
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public bool IsLowStock => CurrentStock < MinLevel; // Getter for UI
        public bool IsOutOfStock => CurrentStock == 0; // Getter for UI
    }
}
