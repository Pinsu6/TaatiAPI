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
    }
}
