using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class KpiDto
    {
        public decimal ThisMonthSales { get; set; }
        public decimal ThisQuarterSales { get; set; }
        public decimal YtdSales { get; set; }
        public decimal AvgOrderValue { get; set; }
    }
}
