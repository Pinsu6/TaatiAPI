using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;
using TatiPharma.Application.IRepositories;
using TatiPharma.Domain.Entities;
using TatiPharma.Infrastructure.Data;
using TatiPharma.Infrastructure.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TatiPharma.Infrastructure.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
       
            private readonly AppDbContext _context;

            public AnalyticsRepository(AppDbContext context)
            {
                _context = context;
            }

            private IQueryable<SalesInvoiceDetail> GetFilteredQuery(
                DateTime start, DateTime end,
                string? region = null, string? category = null)
            {
                var query = _context.SalesInvoiceDetails
                    .Include(d => d.SalesInvoice!)
                        .ThenInclude(i => i!.Customer)
                    .Include(d => d.Drug)
                    .Where(d =>
                        d.SalesInvoice != null &&
                        d.SalesInvoice.BillDate.HasValue &&
                        d.SalesInvoice.BillDate.Value.Date >= start.Date &&
                        d.SalesInvoice.BillDate.Value.Date <= end.Date &&
                        d.IsActive == true &&
                        d.IsDeleted != true &&
                        d.SalesInvoice.IsActive == true &&
                        d.SalesInvoice.IsDeleted != true);

                if (!string.IsNullOrEmpty(region))
                    query = query.Where(d => d.SalesInvoice!.Customer!.Region == region);

                if (!string.IsNullOrEmpty(category))
                    query = query.Where(d => d.Drug!.TheRapeuticclass == category);

                return query;
            }

            public async Task<decimal> GetTotalSalesAsync(DateTime start, DateTime end,
                string? region = null, string? category = null)
            {
                var q = GetFilteredQuery(start, end, region, category);
                return await q.SumAsync(d => d.TotalAmount ?? 0m);
            }

            public async Task<int> GetOrderCountAsync(DateTime start, DateTime end,
                string? region = null, string? category = null)
            {
                var q = GetFilteredQuery(start, end, region, category);
                return await q.Select(d => d.SalesInvoiceId).Distinct().CountAsync();
            }

            public async Task<List<MonthlySalesRaw>> GetMonthlySalesAsync(int year,
                string? region = null, string? category = null)
            {
                var start = new DateTime(year, 1, 1);
                var end = new DateTime(year, 12, 31);

                var q = GetFilteredQuery(start, end, region, category);

                var grouped = await q
                    .GroupBy(d => new
                    {
                        Month = d.SalesInvoice!.BillDate!.Value.Month,
                        Type = d.InvoiceType // From SalesInvoiceDetail
                    })
                    .Select(g => new
                    {
                        g.Key.Month,
                        g.Key.Type,
                        Revenue = g.Sum(x => x.TotalAmount ?? 0m)
                    })
                    .ToListAsync();

                var result = Enumerable.Range(1, 12)
                    .Select(m => new MonthlySalesRaw
                    {
                        Month = m,
                        Retail = grouped
                            .Where(x => x.Month == m && x.Type == "Retail")
                            .Sum(x => x.Revenue),
                        Wholesale = grouped
                            .Where(x => x.Month == m && x.Type == "Wholesale")
                            .Sum(x => x.Revenue)
                    })
                    .ToList();

                return result;
            }

            public async Task<List<TopProductDto>> GetTopProductsAsync(
                DateTime start, DateTime end, int topN,
                string? region = null, string? category = null)
            {
                var q = GetFilteredQuery(start, end, region, category);

                return await q
                    .GroupBy(d => d.Drug!.DrugName ?? "Unknown")
                    .Select(g => new TopProductDto
                    {
                        ProductName = g.Key,
                        Revenue = g.Sum(x => x.TotalAmount ?? 0m)
                    })
                    .OrderByDescending(p => p.Revenue)
                    .Take(topN)
                    .ToListAsync();
            }

            public async Task<List<RegionSalesRaw>> GetRegionSalesAsync(
                DateTime start, DateTime end,
                string? region = null, string? category = null)
            {
                var q = GetFilteredQuery(start, end, region, category);

                return await q
                    .GroupBy(d => d.SalesInvoice!.Customer!.Region ?? "Unknown")
                    .Select(g => new RegionSalesRaw
                    {
                        Region = g.Key,
                        Revenue = g.Sum(x => x.TotalAmount ?? 0m),
                        Orders = g.Select(x => x.SalesInvoiceId).Distinct().Count()
                    })
                    .ToListAsync();
            }
        public async Task<ProductSalesRaw> GetTopProductAsync(DateTime start, DateTime end, string? category = null)
        {
            var q = GetProductFilteredQuery(start, end, category);
            return await q
                .OrderByDescending(d => d.Revenue)
                .FirstOrDefaultAsync() ?? new ProductSalesRaw();
        }

        public async Task<ProductSalesRaw> GetFastestGrowingProductAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, string? category = null)
        {
            var current = await GetProductSalesListAsync(start, end, category);
            var prev = await GetProductSalesListAsync(prevStart, prevEnd, category);
            var joined = current.Select(c => new
            {
                c,
                p = prev.FirstOrDefault(p => p.DrugId == c.DrugId),
            }).Select(j => new ProductSalesRaw
            {
                DrugId = j.c.DrugId,
                Name = j.c.Name,
                Category = j.c.Category,
                Revenue = j.c.Revenue,
                DateCreated = j.c.DateCreated,
                Growth = CalculateGrowth(j.c.Revenue, j.p?.Revenue ?? 0)
            }).OrderByDescending(x => x.Growth).FirstOrDefault() ?? new ProductSalesRaw();

            joined.Revenue = joined.Growth; // Use as growth for KPI
            return joined;
        }

        public async Task<ProductSalesRaw> GetSlowestMovingProductAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, string? category = null)
        {
            var current = await GetProductSalesListAsync(start, end, category);
            var prev = await GetProductSalesListAsync(prevStart, prevEnd, category);
            var joined = current.Select(c => new
            {
                c,
                p = prev.FirstOrDefault(p => p.DrugId == c.DrugId),
            }).Select(j => new ProductSalesRaw
            {
                DrugId = j.c.DrugId,
                Name = j.c.Name,
                Category = j.c.Category,
                Revenue = j.c.Revenue,
                DateCreated = j.c.DateCreated,
                Growth = CalculateGrowth(j.c.Revenue, j.p?.Revenue ?? 0)
            }).OrderBy(x => x.Growth).FirstOrDefault() ?? new ProductSalesRaw();

            joined.Revenue = joined.Growth; // Use as growth
            return joined;
        }

        public async Task<int> GetNewLaunchesCountAsync(DateTime start, DateTime end, string? category = null)
        {
            return await _context.DrugMasters
                .Where(d => d.DateCreated >= start && d.DateCreated <= end)
                .Where(d => string.IsNullOrEmpty(category) || d.TheRapeuticclass == category)
                .CountAsync();
        }

        public async Task<List<TopSkuDto>> GetTopSkusAsync(DateTime start, DateTime end, int topN, string? category = null)
        {
            var q = GetProductFilteredQuery(start, end, category);
            return await q
                .OrderByDescending(d => d.Revenue)
                .Take(topN)
                .Select(d => new TopSkuDto { Name = d.Name, Revenue = d.Revenue })
                .ToListAsync();
        }

        public async Task<List<ProductInsightDto>> GetProductInsightsAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, string? category = null, string? status = null)
        {
            var current = await GetProductSalesListAsync(start, end, category);
            var prev = await GetProductSalesListAsync(prevStart, prevEnd, category);
            var insights = current.Select(c => new
            {
                c,
                p = prev.FirstOrDefault(p => p.DrugId == c.DrugId),
            }).Select(j => new ProductInsightDto
            {
                Name = j.c.Name,
                Category = j.c.Category,
                Revenue = j.c.Revenue,
                Growth = (j.p?.Revenue > 0 ? "+" + ((j.c.Revenue - j.p.Revenue) / j.p.Revenue * 100).ToString("F0") + "%" : "New"),
                Status = DetermineStatus(j.c.DateCreated, CalculateGrowth(j.c.Revenue, j.p?.Revenue ?? 0))
            });

            if (!string.IsNullOrEmpty(status))
            {
                insights = insights.Where(i => i.Status == status);
            }

            return insights.ToList();
        }

        public async Task<List<AiRecommendationDto>> GetAiRecommendationsAsync(DateTime start, DateTime end, string? category = null)
        {
            var sales = await GetProductSalesListAsync(start, end, category);
            var stocks = await GetProductStocksAsync();
            var recos = sales.Select(s => new
            {
                s,
                stock = stocks.FirstOrDefault(st => st.DrugId == s.DrugId)?.RemainStock ?? 0
            }).Select(j => new AiRecommendationDto
            {
                Product = j.s.Name,
                Stock = j.stock,
                Demand = (long)(j.s.Revenue / (j.s.Revenue > 0 ? 100 : 1)), // Simplified avg demand
                Forecast = (long)(j.s.Revenue / (j.s.Revenue > 0 ? 100 : 1) * 1.2m), // +20%
                Recommendation = j.stock < (j.s.Revenue / 100) ? "Reorder " + ((j.s.Revenue / 100) - j.stock) + " units" : "Avoid Reorder"
            }).ToList();

            return recos;
        }

        private async Task<List<ProductSalesRaw>> GetProductSalesListAsync(DateTime start, DateTime end, string? category)
        {
            return await GetProductFilteredQuery(start, end, category).ToListAsync();
        }

        private IQueryable<ProductSalesRaw> GetProductFilteredQuery(DateTime start, DateTime end, string? category)
        {
            return _context.SalesInvoiceDetails
                .Include(d => d.Drug)
                .Where(d => d.SalesInvoice!.BillDate >= start && d.SalesInvoice.BillDate <= end)
                .Where(d => d.IsActive == true && d.IsDeleted != true)
                .GroupBy(d => d.DrugId)
                .Select(g => new ProductSalesRaw
                {
                    DrugId = g.Key ?? 0,
                    Name = g.FirstOrDefault()!.Drug!.DrugName ?? "Unknown",
                    Category = g.FirstOrDefault()!.Drug!.TheRapeuticclass ?? "Unknown",
                    Revenue = g.Sum(d => d.TotalAmount ?? 0m),
                    DateCreated = g.FirstOrDefault()!.Drug!.DateCreated
                })
                .Where(p => string.IsNullOrEmpty(category) || p.Category == category);
        }

        private async Task<List<ProductStockRaw>> GetProductStocksAsync()
        {
            return await _context.PurchaseDetails
                .GroupBy(pd => pd.DrugId)
                .Select(g => new ProductStockRaw
                {
                    DrugId = g.Key ?? 0,
                    RemainStock = g.Sum(pd => pd.RemainStock ?? 0)
                })
                .ToListAsync();
        }

        private string DetermineStatus(DateTime? dateCreated, decimal growth)
        {
            if (dateCreated > DateTime.UtcNow.AddMonths(-3)) return "New";
            if (growth > 10) return "FastMoving";
            return "SlowMoving";
        }

        private decimal CalculateGrowth(decimal current, decimal previous)
        {
            return previous > 0 ? ((current - previous) / previous) * 100m : 0m;
        }

        public async Task<decimal> GetTotalStockValueAsync(string? category = null)
        {
            var q = GetInventoryQuery(category);
            return await q.SumAsync(s => s.RemainStock * s.UnitCost);
        }

        public async Task<int> GetStockOutsCountAsync(string? category = null)
        {
            var q = GetInventoryQuery(category);
            return await q.CountAsync(s => s.RemainStock == 0);
        }

        public async Task<int> GetOverstockAlertsCountAsync(string? category = null)
        {
            var q = GetInventoryQuery(category);
            return await q.CountAsync(s => s.RemainStock > (s.MaxLevel ?? 0));
        }

        public async Task<decimal> GetAvgTurnoverAsync(DateTime start, DateTime end, string? category = null)
        {
            var sales = await GetTotalSalesAsync(start, end, null, category);
            var avgStockValue = await GetTotalStockValueAsync(category) / 2; // Simplified avg
            return avgStockValue > 0 ? sales / avgStockValue : 0m;
        }

        public async Task<List<StockByCategoryDto>> GetStockByCategoryAsync(string? category = null)
        {
            var q = GetInventoryQuery(category);
            return await q
                .GroupBy(s => s.Category)
                .Select(g => new StockByCategoryDto
                {
                    Category = g.Key ?? "Unknown",
                    StockUnits = g.Sum(s => s.RemainStock)
                })
                .ToListAsync();
        }

        public async Task<List<TurnoverRatioDto>> GetMonthlyTurnoverAsync(int year, string? category = null)
        {
            var months = new string[] { "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var result = new List<TurnoverRatioDto>();
            for (int m = 1; m <= 12; m++)
            {
                var monthStart = new DateTime(year, m, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                var sales = await GetTotalSalesAsync(monthStart, monthEnd, null, category);
                var avgStock = await GetTotalStockValueAsync(category) / 2; // Simplified
                var ratio = avgStock > 0 ? sales / avgStock : 0m;
                result.Add(new TurnoverRatioDto { Month = months[m], Ratio = ratio });
            }
            return result;
        }

        public async Task<List<StockAlertDto>> GetStockAlertsAsync(string? category = null)
        {
            var q = GetInventoryQuery(category);
            return await q
                .Where(s => s.RemainStock == 0 || s.RemainStock > (s.MaxLevel ?? 0) || s.RemainStock < (s.MinLevel ?? 0))
                .Select(s => new StockAlertDto
                {
                    Product = s.Name,
                    Category = s.Category,
                    CurrentStock = s.RemainStock,
                    Status = s.RemainStock == 0 ? "Stock-Out" :
                             s.RemainStock > (s.MaxLevel ?? 0) ? "Overstock" :
                             "LowStock"
                })
                .ToListAsync();
        }

        private IQueryable<InventoryStockRaw> GetInventoryQuery(string? category)
        {
            return _context.PurchaseDetails
                .Include(pd => pd.Drug)
                .Where(pd => pd.IsActive == true)
                .GroupBy(pd => pd.DrugId)
                .Select(g => new InventoryStockRaw
                {
                    DrugId = g.Key ?? 0,
                    Name = g.FirstOrDefault()!.Drug!.DrugName ?? "Unknown",
                    Category = g.FirstOrDefault()!.Drug!.TheRapeuticclass ?? "Unknown",
                    RemainStock = g.Sum(pd => pd.RemainStock ?? 0),
                    MinLevel = g.FirstOrDefault()!.Drug!.MinLevel,
                    MaxLevel = g.FirstOrDefault()!.Drug!.MaxLevel,
                    UnitCost = g.FirstOrDefault()!.Drug!.UnitCost ?? 0m
                })
                .Where(s => string.IsNullOrEmpty(category) || s.Category == category);
        }

        public async Task<decimal> GetTotalSalesYtdAsync(DateTime ytdStart, DateTime ytdEnd, string? region, string? category)
        {
            var q = GetFilteredQuery(ytdStart, ytdEnd, region, category);
            return await q.SumAsync(d => d.TotalAmount ?? 0m);
        }

        public async Task<decimal> GetSalesGrowthAsync(DateTime currentStart, DateTime currentEnd, DateTime prevStart, DateTime prevEnd, string? region, string? category)
        {
            var currentSales = await GetTotalSalesAsync(currentStart, currentEnd, region, category);
            var prevSales = await GetTotalSalesAsync(prevStart, prevEnd, region, category);
            return CalculateGrowth(currentSales, prevSales);
        }

        public async Task<int> GetActivePharmaciesCountAsync(string? region, string? search)
        {
            var q = _context.Customers.Where(c => c.BitIsActive == true && c.BitIsDelete != true);
            if (!string.IsNullOrEmpty(region)) q = q.Where(c => c.Region == region);
            if (!string.IsNullOrEmpty(search)) q = q.Where(c => (c.CusFirstname + " " + c.CusLastname).Contains(search) || c.CusCode.Contains(search));
            return await q.CountAsync();
        }

        public async Task<int> GetTotalCustomersCountAsync(string? region, string? search)
        {
            var q = _context.Customers.Where(c => c.BitIsDelete != true);
            if (!string.IsNullOrEmpty(region)) q = q.Where(c => c.Region == region);
            if (!string.IsNullOrEmpty(search)) q = q.Where(c => (c.CusFirstname + " " + c.CusLastname).Contains(search) || c.CusCode.Contains(search));
            return await q.CountAsync();
        }

        public async Task<decimal> GetOrderFulfillmentAsync(DateTime start, DateTime end, string? region, string? category)
        {
            var totalOrders = await GetOrderCountAsync(start, end, region, category);
            var fulfilled = await _context.SalesInvoices
                .Where(i => i.BillDate >= start && i.BillDate <= end && i.PaymentStatus == "Paid")
                .CountAsync();
            return totalOrders > 0 ? (decimal)fulfilled / totalOrders * 100 : 0m;
        }

        public async Task<decimal> GetFulfillmentChangeAsync(DateTime currentStart, DateTime currentEnd, DateTime prevStart, DateTime prevEnd, string? region, string? category)
        {
            var current = await GetOrderFulfillmentAsync(currentStart, currentEnd, region, category);
            var prev = await GetOrderFulfillmentAsync(prevStart, prevEnd, region, category);
            return current - prev;
        }

        public async Task<List<MonthlyRevenueDto>> GetRevenuePerformanceAsync(int year, string? region, string? category)
        {
            var raw = await GetMonthlySalesAsync(year, region, category);
            return raw.Select(r => new MonthlyRevenueDto
            {
                Month = r.Month.ToString(), // or use month name if needed
                Retail = r.Retail,
                Wholesale = r.Wholesale
            }).ToList(); // Reuse from sales
        }

        public async Task<List<ProductCategoryShareDto>> GetProductCategoriesShareAsync(DateTime start, DateTime end, string? category)
        {
            var q = GetFilteredQuery(start, end, null, category);
            var total = await q.SumAsync(d => d.TotalAmount ?? 0m);
            return await q
                .GroupBy(d => d.Drug!.TheRapeuticclass ?? "Unknown")
                .Select(g => new ProductCategoryShareDto
                {
                    Category = g.Key,
                    Share = total > 0 ? g.Sum(d => d.TotalAmount ?? 0m) / total * 100 : 0m
                })
                .ToListAsync();
        }

        public async Task<List<RegionalRevenueDto>> GetRegionalPerformanceAsync(DateTime start, DateTime end, string? region, string? category)
        {
            var q = GetFilteredQuery(start, end, region, category);
            return await q
                .GroupBy(d => d.SalesInvoice!.Customer!.Region ?? "Unknown")
                .Select(g => new RegionalRevenueDto
                {
                    Region = g.Key,
                    Revenue = g.Sum(d => d.TotalAmount ?? 0m)
                })
                .ToListAsync();
        }

        public async Task<List<TopPharmacyDto>> GetTopPharmaciesAsync(DateTime start, DateTime end, string? region, string? category, string? search, int topN = 10)
        {
            var q = GetFilteredQuery(start, end, region, category);
            var grouped = q.GroupBy(d => d.SalesInvoice!.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    Revenue = g.Sum(d => d.TotalAmount ?? 0m)
                });

            var top = await grouped.OrderByDescending(g => g.Revenue).Take(topN).ToListAsync();

            var pharmacies = await _context.Customers
                .Where(c => top.Select(t => t.CustomerId).Contains(c.CustomerId))
                .Where(c => string.IsNullOrEmpty(search) || (c.CusFirstname + " " + c.CusLastname).Contains(search))
                .Select(c => new TopPharmacyDto
                {
                    Name = c.CusFirstname + " " + c.CusLastname ?? "Unknown",
                    Region = c.Region ?? "Unknown",
                    Revenue = 0 // To be filled
                })
                .ToListAsync();

            foreach (var p in pharmacies)
            {
                p.Revenue = top.FirstOrDefault(t => t.CustomerId == _context.Customers.First(c => c.CusFirstname + " " + c.CusLastname == p.Name).CustomerId)?.Revenue ?? 0m;
            }

            return pharmacies;
        }
    }
}
