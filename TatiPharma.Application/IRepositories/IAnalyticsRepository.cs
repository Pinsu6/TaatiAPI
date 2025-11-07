using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;

namespace TatiPharma.Application.IRepositories
{
    public interface IAnalyticsRepository
    {
        Task<decimal> GetTotalSalesAsync(DateTime start, DateTime end, string? region = null, string? category = null);
        Task<int> GetOrderCountAsync(DateTime start, DateTime end, string? region = null, string? category = null);
        Task<List<MonthlySalesRaw>> GetMonthlySalesAsync(int year, string? region = null, string? category = null);
        Task<List<TopProductDto>> GetTopProductsAsync(DateTime start, DateTime end, int topN, string? region = null, string? category = null);
        Task<List<RegionSalesRaw>> GetRegionSalesAsync(DateTime start, DateTime end, string? region = null, string? category = null);
        Task<ProductSalesRaw> GetTopProductAsync(DateTime start, DateTime end, string? category = null);
        Task<ProductSalesRaw> GetFastestGrowingProductAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, string? category = null);
        Task<ProductSalesRaw> GetSlowestMovingProductAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, string? category = null);
        Task<int> GetNewLaunchesCountAsync(DateTime start, DateTime end, string? category = null);
        Task<List<TopSkuDto>> GetTopSkusAsync(DateTime start, DateTime end, int topN, string? category = null);
        Task<List<ProductInsightDto>> GetProductInsightsAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, string? category = null, string? status = null);
        Task<List<AiRecommendationDto>> GetAiRecommendationsAsync(DateTime start, DateTime end, string? category = null);
    }
}
