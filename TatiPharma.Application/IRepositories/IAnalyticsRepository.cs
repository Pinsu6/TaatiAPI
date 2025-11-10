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
        //Task<ProductSalesRaw> GetTopProductAsync(DateTime start, DateTime end, string? category = null);
        //Task<ProductSalesRaw> GetFastestGrowingProductAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, string? category = null);
        //Task<ProductSalesRaw> GetSlowestMovingProductAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, string? category = null);
        //Task<int> GetNewLaunchesCountAsync(DateTime start, DateTime end, string? category = null);
        //Task<List<TopSkuDto>> GetTopSkusAsync(DateTime start, DateTime end, int topN, string? category = null);
        //Task<List<ProductInsightDto>> GetProductInsightsAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, string? category = null, string? status = null);
        //Task<List<AiRecommendationDto>> GetAiRecommendationsAsync(DateTime start, DateTime end, string? category = null);

        Task<ProductSalesRaw> GetTopProductAsync(DateTime start, DateTime end, long? drugTypeId = null);
        Task<ProductSalesRaw> GetFastestGrowingProductAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, long? drugTypeId = null);
        Task<ProductSalesRaw> GetSlowestMovingProductAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, long? drugTypeId = null);
        Task<int> GetNewLaunchesCountAsync(DateTime start, DateTime end, long? drugTypeId = null);
        Task<List<TopSkuDto>> GetTopSkusAsync(DateTime start, DateTime end, int topN, long? drugTypeId = null);
        Task<List<ProductInsightDto>> GetProductInsightsAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, long? drugTypeId = null);
        Task<List<AiRecommendationDto>> GetAiRecommendationsAsync(DateTime start, DateTime end, long? drugTypeId = null);
        //Task<decimal> GetTotalStockValueAsync(string? category = null);
        //Task<int> GetStockOutsCountAsync(string? category = null);
        //Task<int> GetOverstockAlertsCountAsync(string? category = null);
        //Task<decimal> GetAvgTurnoverAsync(DateTime start, DateTime end, string? category = null);
        //Task<List<StockByCategoryDto>> GetStockByCategoryAsync(string? category = null);
        //Task<List<TurnoverRatioDto>> GetMonthlyTurnoverAsync(int year, string? category = null);
        //Task<List<StockAlertDto>> GetStockAlertsAsync(string? category = null);
        Task<decimal> GetTotalSalesYtdAsync(DateTime ytdStart, DateTime ytdEnd, string? region, string? category);
        Task<decimal> GetSalesGrowthAsync(DateTime currentStart, DateTime currentEnd, DateTime prevStart, DateTime prevEnd, string? region, string? category);
        Task<int> GetActivePharmaciesCountAsync(string? region, string? search);
        Task<int> GetTotalCustomersCountAsync(string? region, string? search);
        Task<decimal> GetOrderFulfillmentAsync(DateTime start, DateTime end, string? region, string? category);
        Task<decimal> GetFulfillmentChangeAsync(DateTime currentStart, DateTime currentEnd, DateTime prevStart, DateTime prevEnd, string? region, string? category);
        Task<List<MonthlyRevenueDto>> GetRevenuePerformanceAsync(int year, string? region, string? category);
        Task<List<ProductCategoryShareDto>> GetProductCategoriesShareAsync(DateTime start, DateTime end, string? category);
        Task<List<RegionalRevenueDto>> GetRegionalPerformanceAsync(DateTime start, DateTime end, string? region, string? category);
        Task<List<TopPharmacyDto>> GetTopPharmaciesAsync(DateTime start, DateTime end, string? region, string? category, string? search, int topN = 10);

        IQueryable<ProductSalesRaw> GetProductFilteredQuery(DateTime start, DateTime end, long? drugTypeId = null);
        Task<List<ProductSalesRaw>> GetProductSalesListAsync(DateTime start, DateTime end, long? drugTypeId = null);
        Task<List<ProductStockRaw>> GetProductStocksAsync(); // For AI recos

        Task<decimal> GetTotalStockValueAsync(string? category = null, long? drugTypeId = null);
        Task<int> GetStockOutsCountAsync(string? category = null, long? drugTypeId = null);
        Task<int> GetOverstockAlertsCountAsync(string? category = null, long? drugTypeId = null);
        Task<decimal> GetAvgTurnoverAsync(DateTime start, DateTime end, string? category = null, long? drugTypeId = null);
        Task<List<StockByCategoryDto>> GetStockByCategoryAsync(string? category = null, long? drugTypeId = null);
        Task<List<TurnoverRatioDto>> GetMonthlyTurnoverAsync(int year, string? category = null, long? drugTypeId = null);
        Task<List<StockAlertDto>> GetStockAlertsAsync(string? category = null, long? drugTypeId = null);
    }
}
