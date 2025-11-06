using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class CustomerDetailDto : CustomerResponseDto
    {
        public CustomerTypeDto? CustomerType { get; set; }
        public EmployeeDto? Employee { get; set; }
    }
}
