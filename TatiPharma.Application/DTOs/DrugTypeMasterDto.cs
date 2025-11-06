using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class DrugTypeMasterDto
    {
        public long DrugTypeId { get; set; }
        public string DrugTypeName { get; set; } = string.Empty;
        public bool? BitIsActive { get; set; }
    }
}
