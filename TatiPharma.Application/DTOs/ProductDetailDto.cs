using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductDetailDto : ProductResponseDto
    {
        public string? TheRapeuticclass { get; set; } = string.Empty;
        public string? DrugNdccode { get; set; } = string.Empty;
        public string? DrugType { get; set; } = string.Empty;  // Raw type
        public decimal? MaxLevel { get; set; }  // Already in base, but ensure
        public decimal? MinLevel { get; set; }
        public bool? BitIsDelete { get; set; }
        public long? DrugTypeId { get; set; }
        public long? DosageId { get; set; }
        public long? ManufacturerId { get; set; }
        public long? UdiId { get; set; }
        public long? UomId { get; set; }
        public long? TaxDetailsId { get; set; }
        public long? Taxid { get; set; }

        // Full Navigation Objects (not just names)
        public DrugTypeMasterDto? DrugTypeMaster { get; set; }
        public DosageFormDto? DosageForm { get; set; }
        public ManufacturerDto? Manufacturer { get; set; }

        public StockSummaryDto? StockSummary { get; set; }
        public List<BatchSummaryDto> ActiveBatches { get; set; } = new();
        public PriceBreakdownDto? Pricing { get; set; }

        // === Audit & Status ===
        public bool IsNarcotic => Narcotics == true;
        public bool IsActive => BitIsActive == true;
    }
}
