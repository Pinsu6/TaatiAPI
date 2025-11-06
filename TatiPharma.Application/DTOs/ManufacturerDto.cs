using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ManufacturerDto
    {
        public long ManufacturerId { get; set; }
        public string ManufacturerName { get; set; } = string.Empty;
        public string? Mfg { get; set; } = string.Empty;
        public string? MfgPerson { get; set; } = string.Empty;
        public string? MfgEmail { get; set; } = string.Empty;
        public string? MfgMobile { get; set; } = string.Empty;
    }
}
