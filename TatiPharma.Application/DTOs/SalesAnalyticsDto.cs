using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class SalesAnalyticsDto
    {
        public KpiDto Kpis { get; set; } = new KpiDto();
        public List<MonthlySalesTrendDto> SalesTrend { get; set; } = new List<MonthlySalesTrendDto>();
        public List<TopProductDto> TopProducts { get; set; } = new List<TopProductDto>();
        public List<RegionSalesDto> SalesByRegion { get; set; } = new List<RegionSalesDto>();
    }
}
