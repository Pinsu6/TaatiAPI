using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductInsightsFilterRequestDto
    {
        public long? DrugTypeId { get; set; } // Filter by DrugTypeMaster ID (replaces Category string)
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
