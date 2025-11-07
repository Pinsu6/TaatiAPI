using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class MonthlySalesTrendDto
    {
        public string Month { get; set; } = string.Empty;
        public decimal Retail { get; set; }
        public decimal Wholesale { get; set; }
    }
}
