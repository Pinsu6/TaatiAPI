using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class StockByCategoryDto
    {
        public string Category { get; set; } = string.Empty;
        public long StockUnits { get; set; }
    }
}
