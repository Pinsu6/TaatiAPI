using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;
using TatiPharma.Application.IRepositories;
using TatiPharma.Application.IServices;

namespace TatiPharma.Application.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsRepository _salesAnalyticsRepository;
        private readonly IMapper _mapper;

        public AnalyticsService(IAnalyticsRepository salesAnalyticsRepository, IMapper mapper)
        {
            _salesAnalyticsRepository = salesAnalyticsRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<SalesAnalyticsDto>> GetSalesAnalyticsAsync(SalesAnalyticsFilterRequestDto request)
        {
            try
            {
                var now = DateTime.UtcNow;

                // KPIs (fixed, no filters)
                var kpis = new KpiDto();

                // This Month
                var monthStart = new DateTime(now.Year, now.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                kpis.ThisMonthSales = await _salesAnalyticsRepository.GetTotalSalesAsync(monthStart, monthEnd);

                // This Quarter
                var quarter = ((now.Month - 1) / 3) + 1;
                var quarterStart = new DateTime(now.Year, (quarter - 1) * 3 + 1, 1);
                var quarterEnd = quarterStart.AddMonths(3).AddDays(-1);
                kpis.ThisQuarterSales = await _salesAnalyticsRepository.GetTotalSalesAsync(quarterStart, quarterEnd);

                // YTD
                var ytdStart = new DateTime(now.Year, 1, 1);
                var ytdEnd = now;
                kpis.YtdSales = await _salesAnalyticsRepository.GetTotalSalesAsync(ytdStart, ytdEnd);
                var ytdOrders = await _salesAnalyticsRepository.GetOrderCountAsync(ytdStart, ytdEnd);
                kpis.AvgOrderValue = ytdOrders > 0 ? kpis.YtdSales / ytdOrders : 0;

                // Period for filtered data
                var (start, end) = GetPeriodDates(request.Period, now);
                var (prevStart, prevEnd) = GetPreviousPeriodDates(start, request.Period);

                // Sales Trend (current year, filtered by region and category)
                var year = now.Year;
                var monthlyRaw = await _salesAnalyticsRepository.GetMonthlySalesAsync(year, request.Region, request.ProductCategory);
                var months = new string[] { "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                var salesTrend = new List<MonthlySalesTrendDto>();
                for (int m = 1; m <= 12; m++)
                {
                    var raw = monthlyRaw.FirstOrDefault(r => r.Month == m);
                    salesTrend.Add(new MonthlySalesTrendDto
                    {
                        Month = months[m],
                        Retail = raw?.Retail ?? 0,
                        Wholesale = raw?.Wholesale ?? 0
                    });
                }

                // Top Products (for period, filtered)
                var topProducts = await _salesAnalyticsRepository.GetTopProductsAsync(start, end, 5, request.Region, request.ProductCategory);

                // Sales by Region (for period, filtered, with growth)
                var currentRegions = await _salesAnalyticsRepository.GetRegionSalesAsync(start, end, request.Region, request.ProductCategory);
                var prevRegions = await _salesAnalyticsRepository.GetRegionSalesAsync(prevStart, prevEnd, request.Region, request.ProductCategory);
                var salesByRegion = currentRegions.Select(cr =>
                {
                    var pr = prevRegions.FirstOrDefault(p => p.Region == cr.Region);
                    decimal prevRev = pr?.Revenue ?? 0;
                    decimal growth = prevRev > 0 ? ((cr.Revenue - prevRev) / prevRev) * 100 : 0;
                    return new RegionSalesDto
                    {
                        Region = cr.Region,
                        Revenue = cr.Revenue,
                        Orders = cr.Orders,
                        Growth = growth
                    };
                }).ToList();

                var result = new SalesAnalyticsDto
                {
                    Kpis = kpis,
                    SalesTrend = salesTrend,
                    TopProducts = topProducts,
                    SalesByRegion = salesByRegion
                };

                return ApiResponse<SalesAnalyticsDto>.SuccessResult(result, "Sales analytics retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<SalesAnalyticsDto>.ErrorResult(new List<string> { ex.Message });
            }
        }

        private (DateTime, DateTime) GetPeriodDates(string period, DateTime now)
        {
            switch (period)
            {
                case "ThisMonth":
                    return (new DateTime(now.Year, now.Month, 1), new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1));
                case "LastMonth":
                    var last = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
                    return (last, last.AddMonths(1).AddDays(-1));
                case "Last3Months":
                    var last3 = new DateTime(now.Year, now.Month, 1).AddMonths(-3);
                    return (last3, new DateTime(now.Year, now.Month, 1).AddDays(-1));
                case "ThisYear":
                    return (new DateTime(now.Year, 1, 1), new DateTime(now.Year, 12, 31));
                default:
                    return (new DateTime(now.Year, now.Month, 1), new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1));
            }
        }

        private (DateTime, DateTime) GetPreviousPeriodDates(DateTime currentStart, string period)
        {
            int monthsToSubtract = period switch
            {
                "ThisMonth" or "LastMonth" => 1,
                "Last3Months" => 3,
                "ThisYear" => 12,
                _ => 1
            };
            var prevStart = currentStart.AddMonths(-monthsToSubtract);
            var prevEnd = prevStart.AddMonths(monthsToSubtract).AddDays(-1);
            return (prevStart, prevEnd);
        }

        public async Task<ApiResponse<ProductInsightsDto>> GetProductInsightsAsync(ProductInsightsFilterRequestDto request)
        {
            try
            {
                var now = DateTime.UtcNow;
                var start = request.StartDate ?? new DateTime(now.Year, now.Month, 1).AddMonths(-3); // Default last 3 months
                var end = request.EndDate ?? now;
                var prevStart = start.AddMonths(-(int)(end - start).TotalDays / 30); // Approx previous period
                var prevEnd = start.AddDays(-1);

                // KPIs
                var kpis = new ProductKpiDto();
                var top = await _salesAnalyticsRepository.GetTopProductAsync(start, end, request.Category);
                kpis.TopProductName = top.Name;
                kpis.TopProductRevenue = top.Revenue;

                var fastest = await _salesAnalyticsRepository.GetFastestGrowingProductAsync(start, end, prevStart, prevEnd, request.Category);
                kpis.FastestGrowingName = fastest.Name;
                kpis.FastestGrowingGrowth = CalculateGrowth(top.Revenue, fastest.Revenue); // Simplified

                var slowest = await _salesAnalyticsRepository.GetSlowestMovingProductAsync(start, end, prevStart, prevEnd, request.Category);
                kpis.SlowestMovingName = slowest.Name;
                kpis.SlowestMovingGrowth = CalculateGrowth(slowest.Revenue, slowest.Revenue); // Simplified

                kpis.NewLaunchesCount = await _salesAnalyticsRepository.GetNewLaunchesCountAsync(start, end, request.Category);

                // Top SKUs
                var topSkus = await _salesAnalyticsRepository.GetTopSkusAsync(start, end, 10, request.Category);

                // Lifecycle (general, dummy based on aggregates) 
                var lifecycle = new List<ProductLifecycleDto>
        {
            new() { Stage = "Launch", Sales = 200000 },
            new() { Stage = "Growth", Sales = 800000 },
            new() { Stage = "Maturity", Sales = 1200000 },
            new() { Stage = "Decline", Sales = 600000 }
        };

                // Products Table
                var products = await _salesAnalyticsRepository.GetProductInsightsAsync(start, end, prevStart, prevEnd, request.Category, request.Status);

                // AI Recommendations
                var aiRecos = await _salesAnalyticsRepository.GetAiRecommendationsAsync(start, end, request.Category);

                var result = new ProductInsightsDto
                {
                    Kpis = kpis,
                    TopSkus = topSkus,
                    LifecycleStages = lifecycle,
                    Products = products,
                    AiRecommendations = aiRecos
                };

                return ApiResponse<ProductInsightsDto>.SuccessResult(result, "Product insights retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductInsightsDto>.ErrorResult(new List<string> { ex.Message });
            }
        }

        private decimal CalculateGrowth(decimal current, decimal prev)
        {
            return prev > 0 ? ((current - prev) / prev) * 100 : 0;
        }
        public async Task<ApiResponse<InventoryAnalyticsDto>> GetInventoryAnalyticsAsync(InventoryFilterRequestDto request)
        {
            try
            {
                var now = DateTime.UtcNow;
                var start = request.StartDate ?? new DateTime(now.Year, 1, 1);
                var end = request.EndDate ?? now;

                // KPIs
                var kpis = new InventoryKpiDto
                {
                    TotalStockValue = await _salesAnalyticsRepository.GetTotalStockValueAsync(request.Category),
                    StockOuts = await _salesAnalyticsRepository.GetStockOutsCountAsync(request.Category),
                    OverstockAlerts = await _salesAnalyticsRepository.GetOverstockAlertsCountAsync(request.Category),
                    AvgTurnover = await _salesAnalyticsRepository.GetAvgTurnoverAsync(start, end, request.Category)
                };

                // Stock by Category
                var stockByCategory = await _salesAnalyticsRepository.GetStockByCategoryAsync(request.Category);

                // Turnover Ratio
                var turnoverRatio = await _salesAnalyticsRepository.GetMonthlyTurnoverAsync(now.Year, request.Category);

                // Stock Alerts
                var stockAlerts = await _salesAnalyticsRepository.GetStockAlertsAsync(request.Category);

                var result = new InventoryAnalyticsDto
                {
                    Kpis = kpis,
                    StockByCategory = stockByCategory,
                    TurnoverRatio = turnoverRatio,
                    StockAlerts = stockAlerts
                };

                return ApiResponse<InventoryAnalyticsDto>.SuccessResult(result, "Inventory analytics retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<InventoryAnalyticsDto>.ErrorResult(new List<string> { ex.Message });
            }
        }
        public async Task<ApiResponse<DashboardAnalyticsDto>> GetDashboardAnalyticsAsync(DashboardFilterRequestDto request)
        {
            try
            {
                var now = DateTime.UtcNow;
                var (start, end) = GetPeriodDates(request.Period, now);
                var (prevStart, prevEnd) = GetPreviousPeriodDates(start, request.Period);
                var ytdStart = new DateTime(now.Year, 1, 1);
                var ytdEnd = now;

                // KPIs
                var kpis = new DashboardKpiDto
                {
                    TotalSalesYtd = await _salesAnalyticsRepository.GetTotalSalesYtdAsync(ytdStart, ytdEnd, request.Region, request.ProductCategory),
                    SalesGrowth = await _salesAnalyticsRepository.GetSalesGrowthAsync(start, end, prevStart, prevEnd, request.Region, request.ProductCategory),
                    ActivePharmacies = await _salesAnalyticsRepository.GetActivePharmaciesCountAsync(request.Region, request.Search),
                    TotalCustomers = await _salesAnalyticsRepository.GetTotalCustomersCountAsync(request.Region, request.Search),
                    OrderFulfillment = await _salesAnalyticsRepository.GetOrderFulfillmentAsync(start, end, request.Region, request.ProductCategory),
                    FulfillmentChange = await _salesAnalyticsRepository.GetFulfillmentChangeAsync(start, end, prevStart, prevEnd, request.Region, request.ProductCategory)
                };

                // Revenue Performance
                var revenuePerformance = await _salesAnalyticsRepository.GetRevenuePerformanceAsync(now.Year, request.Region, request.ProductCategory);

                // Product Categories
                var productCategories = await _salesAnalyticsRepository.GetProductCategoriesShareAsync(start, end, request.ProductCategory);

                // Regional Performance
                var regionalPerformance = await _salesAnalyticsRepository.GetRegionalPerformanceAsync(start, end, request.Region, request.ProductCategory);

                // Top Pharmacies
                var topPharmacies = await _salesAnalyticsRepository.GetTopPharmaciesAsync(start, end, request.Region, request.ProductCategory, request.Search);

                var result = new DashboardAnalyticsDto
                {
                    Kpis = kpis,
                    RevenuePerformance = revenuePerformance,
                    ProductCategories = productCategories,
                    RegionalPerformance = regionalPerformance,
                    TopPharmacies = topPharmacies
                };

                return ApiResponse<DashboardAnalyticsDto>.SuccessResult(result, "Dashboard analytics retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<DashboardAnalyticsDto>.ErrorResult(new List<string> { ex.Message });
            }
        }
    }
}
