using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class EmployeeDto
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmpShort { get; set; } = string.Empty;
        public string? EmpPerson { get; set; }
        public string? EmpPosition { get; set; }
        public string? EmpEmail { get; set; }
        public string? EmpMobile { get; set; }
        public string? EmpAddress { get; set; }
        public string? EmpType { get; set; }
        public bool? BitIsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? BitIsDelete { get; set; }
        public string? EmpPassword { get; set; }  // Caution: Sensitive, mask if needed
        public long? BintUserId { get; set; }
        public DateTime? EmpStartDate { get; set; }
    }
}
