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
        public long CurrentStock => TotalPurchased - TotalSold;
        public int MinLevel { get; set; } = 0; // Set from product.MinLevel in Service
        public bool IsLowStock => CurrentStock <= MinLevel;
        public bool IsOutOfStock => CurrentStock <= 0;
    }
}
