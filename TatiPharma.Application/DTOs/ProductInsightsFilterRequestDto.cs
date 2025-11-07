using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductInsightsFilterRequestDto
    {
        public string? Category { get; set; } // TheRapeuticclass
        public string? Status { get; set; } // FastMoving, SlowMoving, New
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
