using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductDropdownDto
    {
        public long drugId { get; set; }
        public string drugName { get; set; } = string.Empty;
    }
}
