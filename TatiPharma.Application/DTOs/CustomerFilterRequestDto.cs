using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class CustomerFilterRequestDto : PaginationRequestDto
    {
        public string? Search { get; set; }  // Search by name (firstname + lastname) or code or email
        public bool? IsActive { get; set; }  // Filter by status (active/inactive)
        // Add more filters later: e.g., public long? CusTypeId { get; set; }
    }
}
