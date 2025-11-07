using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatiPharma.Application.DTOs
{
    public class AiRecommendationDto
    {
        public string Product { get; set; } = string.Empty;
        public long Stock { get; set; }
        public long Demand { get; set; }
        public long Forecast { get; set; }
        public string Recommendation { get; set; } = string.Empty;
    }
}
