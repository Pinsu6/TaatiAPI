using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductSalesRaw
    {
        public long DrugId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public DateTime? DateCreated { get; set; }
        public decimal Growth { get; set; }
    }
}
