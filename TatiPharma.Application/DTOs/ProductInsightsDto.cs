using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class ProductInsightsDto
    {
        public ProductKpiDto Kpis { get; set; } = new ProductKpiDto();
        public List<TopSkuDto> TopSkus { get; set; } = new List<TopSkuDto>();
        public List<ProductInsightDto> Products { get; set; } = new List<ProductInsightDto>();
        public List<AiRecommendationDto> AiRecommendations { get; set; } = new List<AiRecommendationDto>();
    }
}
