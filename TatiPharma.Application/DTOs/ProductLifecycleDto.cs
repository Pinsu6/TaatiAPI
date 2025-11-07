using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductLifecycleDto
    {
        public string Stage { get; set; } = string.Empty; // Launch, Growth, Maturity, Decline
        public decimal Sales { get; set; }
    }
}
