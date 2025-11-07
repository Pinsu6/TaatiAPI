using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class CustomerResponseDto
    {
        public long CustomerId { get; set; }
        public string CusCode { get; set; } = string.Empty;
        public string CusFirstname { get; set; } = string.Empty;
        public string CusLastname { get; set; } = string.Empty;
        public string CusMobileno { get; set; } = string.Empty;
        public string CusPhonenoO { get; set; } = string.Empty;
        public string CusPhonenoR { get; set; } = string.Empty;
        public string CusEmail { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public long? EmployeeId { get; set; }
        public bool? BitIsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? BitIsDelete { get; set; }
        public decimal? StoreAmtremain { get; set; }
        public decimal? StoreAmtused { get; set; }
        public string Region { get; set; } = string.Empty;
        public string Pbsllicense { get; set; } = string.Empty;
        public string LicenseType { get; set; } = string.Empty;
        public DateTime? LicenseExpiry { get; set; }
        public long? CusTypeId { get; set; }
        public decimal? Creditlim { get; set; }
        public int? Creditdays { get; set; }
    }
}
