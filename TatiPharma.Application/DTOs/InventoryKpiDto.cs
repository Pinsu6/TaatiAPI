using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class InventoryKpiDto
    {
        public decimal TotalStockValue { get; set; }
        public int StockOuts { get; set; }
        public int OverstockAlerts { get; set; }
        public decimal AvgTurnover { get; set; }
    }
}
