using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class StockAlertDto
    {
        public string Product { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public long CurrentStock { get; set; }
        public string Status { get; set; } = string.Empty; // Stock-Out, Overstock, LowStock
    }
}
