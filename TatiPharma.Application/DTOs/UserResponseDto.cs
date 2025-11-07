using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class UserResponseDto
    {
        public long BintUserId { get; set; }
        public string UserName { get; set; } = string.Empty;  // ← safe default
        public string EmailAddress { get; set; } = string.Empty;
        public string StrFirstName { get; set; } = string.Empty;
        public string StrLastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string StrMobileNumber { get; set; } = string.Empty;
        public bool BitIsActive { get; set; }
        public string Gender { get; set; } = string.Empty;
        public bool? IsMain { get; set; }
    }
}
