using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class BatchSummaryDto
    {
        public string BatchNo { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public long RemainingQty { get; set; }
        public bool IsExpiringSoon => ExpiryDate < DateTime.Today.AddMonths(3);
    }
}
