using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class InventoryAnalyticsDto
    {
        public InventoryKpiDto Kpis { get; set; } = new InventoryKpiDto();
        public List<StockByCategoryDto> StockByCategory { get; set; } = new List<StockByCategoryDto>();
        public List<TurnoverRatioDto> TurnoverRatio { get; set; } = new List<TurnoverRatioDto>();
        public List<StockAlertDto> StockAlerts { get; set; } = new List<StockAlertDto>();
    }
}
