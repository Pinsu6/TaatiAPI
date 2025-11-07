using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class CustomerTypeDto
    {
        public long CusTypeId { get; set; }
        public string CusTypeName { get; set; } = string.Empty;
        public bool? BitIsActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? BitIsDelete { get; set; }
    }
}
