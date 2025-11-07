using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class DashboardKpiDto
    {
        public decimal TotalSalesYtd { get; set; }
        public decimal SalesGrowth { get; set; }
        public int ActivePharmacies { get; set; }
        public int TotalCustomers { get; set; }
        public decimal OrderFulfillment { get; set; }
        public decimal FulfillmentChange { get; set; }
    }
}
