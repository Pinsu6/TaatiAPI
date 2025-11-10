using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class RecentOrderDto
    {
        public long OrderId { get; set; }
        public DateTime? Date { get; set; }
        public string Customer { get; set; } = string.Empty; // Full name
        public long Quantity { get; set; } // For this drug
        public decimal Total { get; set; } // Order total
    }
}
