using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class DosageFormDto
    {
        public long DosageId { get; set; }
        public string DosageFormname { get; set; } = string.Empty;
        public string? DosageType { get; set; } = string.Empty;
        public bool? BitIsActive { get; set; }
    }
}
