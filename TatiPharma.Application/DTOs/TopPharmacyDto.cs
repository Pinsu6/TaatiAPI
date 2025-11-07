using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class TopPharmacyDto
    {
        public string Name { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }
}
