using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TatiPharma.Application.DTOs;
using TatiPharma.Application.IServices;

namespace TatiPharma.API.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _salesAnalyticsService;

        public AnalyticsController(IAnalyticsService salesAnalyticsService)
        {
            _salesAnalyticsService = salesAnalyticsService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<SalesAnalyticsDto>>> GetAnalytics([FromQuery] SalesAnalyticsFilterRequestDto request)
        {
            var response = await _salesAnalyticsService.GetSalesAnalyticsAsync(request);
            return Ok(response);
        }

        [HttpGet("product-insights")]
        public async Task<ActionResult<ApiResponse<ProductInsightsDto>>> GetProductInsights([FromQuery] ProductInsightsFilterRequestDto request)
        {
            var response = await _salesAnalyticsService.GetProductInsightsAsync(request);
            return Ok(response);
        }
    }
}
