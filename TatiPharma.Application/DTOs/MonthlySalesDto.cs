using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class MonthlySalesDto
    {
        public string Month { get; set; } = string.Empty; // e.g., "Jan"
        public decimal Amount { get; set; }
    }
}
