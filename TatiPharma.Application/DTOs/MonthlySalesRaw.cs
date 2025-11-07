using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class MonthlySalesRaw
    {
        public int Month { get; set; }
        public decimal Retail { get; set; }
        public decimal Wholesale { get; set; }
    }
}
