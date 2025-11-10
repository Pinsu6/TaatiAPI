using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class StockMovementDto
    {
        public DateTime? Date { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
