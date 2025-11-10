using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class PriceBreakdownDto
    {
        public decimal UnitCost { get; set; }
        public decimal MarginPercent { get; set; }
        public decimal MarginAmount { get; set; }
        public decimal SalePrice { get; set; }
    }
}
