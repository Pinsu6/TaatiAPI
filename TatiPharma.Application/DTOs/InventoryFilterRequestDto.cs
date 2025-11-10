using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class InventoryFilterRequestDto
    {
        public string? Category { get; set; }           // DrugTypeName
        public long? DrugTypeId { get; set; }           // ← NEW
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
