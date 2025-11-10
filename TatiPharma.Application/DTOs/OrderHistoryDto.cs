using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class OrderHistoryDto
    {
        public long OrderId { get; set; }
        public DateTime? Date { get; set; }
        public int Items { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
