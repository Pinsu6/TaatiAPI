using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class EngagementDto
    {
        public string Type { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
