using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class CustomerDetailDto : CustomerResponseDto
    {
        public CustomerTypeDto? CustomerType { get; set; }
        public EmployeeDto? Employee { get; set; }
        public int TotalOrders { get; set; }
        public decimal LifetimeValue { get; set; }
        public DateTime? LastPurchase { get; set; }
        public int ActivePolicies { get; set; }
        public List<OrderHistoryDto> OrderHistory { get; set; } = new List<OrderHistoryDto>();
        public List<MonthlyPurchaseDto> PurchaseTrend { get; set; } = new List<MonthlyPurchaseDto>();
        public List<CategorySplitDto> CategorySplit { get; set; } = new List<CategorySplitDto>();
        public List<EngagementDto> Engagement { get; set; } = new List<EngagementDto>();
    }
}
