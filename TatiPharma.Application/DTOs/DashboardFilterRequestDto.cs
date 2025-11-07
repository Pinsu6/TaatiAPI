using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class DashboardFilterRequestDto
    {
        public string? Search { get; set; }
        public string? Region { get; set; }
        public string? ProductCategory { get; set; }
        public string Period { get; set; } = "Last30Days"; // Last7Days, Last90Days, etc.
    }
}
