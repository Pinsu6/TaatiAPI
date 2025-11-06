using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductFilterRequestDto : PaginationRequestDto
    {
        public string? Search { get; set; }           // Search: Name, Code, QuickCode, NDC
        public bool? IsActive { get; set; }           // Filter by active status
        public long? DrugTypeId { get; set; }         // Optional: Filter by type
        public long? DosageId { get; set; }           // Optional: Filter by dosage
        public long? ManufacturerId { get; set; }     // Optional: Filter by manufacturer
    }
}
