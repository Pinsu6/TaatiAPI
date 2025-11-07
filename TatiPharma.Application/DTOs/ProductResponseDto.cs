using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductResponseDto
    {
        public long DrugId { get; set; }
        public string DrugCode { get; set; } = string.Empty;
        public string DrugQuickcode { get; set; } = string.Empty;
        public string DrugName { get; set; } = string.Empty;
        public string DrugShort { get; set; } = string.Empty;
        public string Strength { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string QuantityPack { get; set; } = string.Empty;
        public int? MaxLevel { get; set; }
        public int? MinLevel { get; set; }
        public bool? Narcotics { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? Margin { get; set; }
        public bool? BitIsActive { get; set; }
        public DateTime? DateCreated { get; set; }

        // Navigation (included)
        public string? DrugTypeName { get; set; }
        public string? DosageFormName { get; set; }
        public string? ManufacturerName { get; set; }
    }
}
