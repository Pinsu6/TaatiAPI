using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class SalesAnalyticsFilterRequestDto
    {
        public string? City { get; set; }
        public string? ProductCategory { get; set; }
        public string Period { get; set; } = "ThisYear";
    }
}
