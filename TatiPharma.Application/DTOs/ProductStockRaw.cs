using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductStockRaw
    {
        public long? DrugId { get; set; }
        public long RemainStock { get; set; }
    }
}
