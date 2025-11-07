using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductInsightDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public string Growth { get; set; } = string.Empty; // e.g., "+10%"
        public string Status { get; set; } = string.Empty; // FastMoving, SlowMoving, New
    }
}
