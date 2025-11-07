using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductKpiDto
    {
        public string TopProductName { get; set; } = string.Empty;
        public decimal TopProductRevenue { get; set; }
        public string FastestGrowingName { get; set; } = string.Empty;
        public decimal FastestGrowingGrowth { get; set; }
        public string SlowestMovingName { get; set; } = string.Empty;
        public decimal SlowestMovingGrowth { get; set; }
        public int NewLaunchesCount { get; set; }
    }
}
