using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class RegionSalesDto
    {
        public string Region { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
        public decimal Growth { get; set; }
    }
}
