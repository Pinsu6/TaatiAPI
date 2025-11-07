using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class DashboardAnalyticsDto
    {
        public DashboardKpiDto Kpis { get; set; } = new DashboardKpiDto();
        public List<MonthlyRevenueDto> RevenuePerformance { get; set; } = new List<MonthlyRevenueDto>();
        public List<ProductCategoryShareDto> ProductCategories { get; set; } = new List<ProductCategoryShareDto>();
        public List<RegionalRevenueDto> RegionalPerformance { get; set; } = new List<RegionalRevenueDto>();
        public List<TopPharmacyDto> TopPharmacies { get; set; } = new List<TopPharmacyDto>();
    }
}
