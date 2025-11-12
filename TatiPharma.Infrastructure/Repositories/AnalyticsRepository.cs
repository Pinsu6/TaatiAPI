using DocumentFormat.OpenXml.Wordprocessing;
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
    public class AnalyticsRepository :  IAnalyticsRepository
    {
       
            private readonly AppDbContext _context;

            public AnalyticsRepository(AppDbContext context)
            {
                _context = context;
            }


        

        private IQueryable<SalesInvoiceDetail> GetFilteredQuery(
            DateTime start, DateTime end,
            string? city = null, string? category = null)
        {
            var q = _context.SalesInvoiceDetails
                .Include(d => d.SalesInvoice!)
                    .ThenInclude(i => i.Customer!)
                        .ThenInclude(c => c.CustomerType)
                .Include(d => d.Drug!)
                    .ThenInclude(dm => dm.DrugTypeMaster)
                .AsNoTracking() // FIXED: Perf for reads
                .Where(d =>
                    d.SalesInvoice != null &&
                    d.SalesInvoice.BillDate.HasValue &&
                    d.SalesInvoice.BillDate.Value.Date >= start.Date &&
                    d.SalesInvoice.BillDate.Value.Date <= end.Date &&
                    d.IsActive == true && d.IsDeleted != true &&
                    d.SalesInvoice.IsActive == true && d.SalesInvoice.IsDeleted != true);
            if (!string.IsNullOrEmpty(city))
                q = q.Where(d => d.SalesInvoice!.Customer!.City == city);
            if (!string.IsNullOrEmpty(category))
                q = q.Where(d => d.Drug!.DrugTypeMaster!.DrugTypeName == category);
            return q;
        }

        

        public async Task<decimal> GetTotalSalesAsync(DateTime start, DateTime end, string? city = null, string? category = null)
        {
            var q = GetFilteredQuery(start, end, city, category);
            return await q.SumAsync(d => d.TotalAmount ?? 0m);
        }

        

        public async Task<int> GetOrderCountAsync(DateTime start, DateTime end, string? city = null, string? category = null)
        {
            var q = GetFilteredQuery(start, end, city, category);
            return await q.Select(d => d.SalesInvoiceId).Distinct().CountAsync();
        }

        
        public async Task<List<MonthlySalesRaw>> GetMonthlySalesAsync(int year, string? city = null, string? category = null)
        {
            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);
            var q = GetFilteredQuery(start, end, city, category);
            var grouped = await q
                .GroupBy(d => new
                {
                    Month = d.SalesInvoice!.BillDate!.Value.Month,
                    Type = d.SalesInvoice.Customer!.CustomerType!.CusTypeName ?? "Wholesale"
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
                    Retail = 0m, // Always 0
                    Wholesale = grouped.Where(x => x.Month == m).Sum(x => x.Revenue) // All to Wholesale
                })
                .ToList();
            return result;
        }

       

        public async Task<List<TopProductDto>> GetTopProductsAsync(DateTime start, DateTime end, int topN, string? city = null, string? category = null)
        {
            var q = GetFilteredQuery(start, end, city, category);
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



        public async Task<List<RegionSalesRaw>> GetRegionSalesAsync(DateTime start, DateTime end, string? city = null, string? category = null)
        {
            var q = GetFilteredQuery(start, end, city, category);
            return await q
                .GroupBy(d => d.SalesInvoice!.Customer!.Region ?? d.SalesInvoice.Customer.City ?? "Unknown") // FIXED: Prefer Region, fallback to City
                .Select(g => new RegionSalesRaw
                {
                    Region = g.Key,
                    Revenue = g.Sum(x => x.TotalAmount ?? 0m),
                    Orders = g.Select(x => x.SalesInvoiceId).Distinct().Count()
                })
                .ToListAsync();
        }

        public async Task<decimal> GetTotalStockValueAsync(string? category = null, long? drugTypeId = null)
        {
            var q = GetInventoryQuery(category, drugTypeId);
            return await q.SumAsync(s => s.RemainStock * s.UnitCost);
        }

      

        public async Task<int> GetStockOutsCountAsync(string? category = null, long? drugTypeId = null)
        {
            var q = GetInventoryQuery(category, drugTypeId);
            return await q.CountAsync(s => s.RemainStock == 0);
        }

      

        public async Task<int> GetOverstockAlertsCountAsync(string? category = null, long? drugTypeId = null)
        {
            var q = GetInventoryQuery(category, drugTypeId);
            return await q.CountAsync(s => s.RemainStock > (s.MaxLevel ?? 0));
        }


        public async Task<decimal> GetAvgTurnoverAsync(DateTime start, DateTime end, string? category = null, long? drugTypeId = null)
        {
            var cogs = await _context.SalesInvoiceDetails
                .Include(sid => sid.PurchaseDetail)
                .Include(sid => sid.Drug)
                .Where(sid => sid.SalesInvoice!.BillDate >= start && sid.SalesInvoice.BillDate <= end)
                .Where(sid => sid.IsActive == true && sid.IsDeleted != true)
                .Where(sid => drugTypeId == null || sid.Drug!.DrugTypeId == drugTypeId)
                .Where(sid => string.IsNullOrEmpty(category) || sid.Drug!.DrugTypeMaster!.DrugTypeName == category)
                .SumAsync(sid => (sid.Qty ?? 0) * (sid.PurchaseDetail!.UnitCost ?? 0m));

            var avgInventoryValue = await GetTotalStockValueAsync(category, drugTypeId) / 2m;
            return avgInventoryValue > 0 ? cogs / avgInventoryValue : 0m;
        }

       
        public async Task<List<StockByCategoryDto>> GetStockByCategoryAsync(string? category = null, long? drugTypeId = null)
        {
            var q = GetInventoryQuery(category, drugTypeId);
            return await q
                .GroupBy(s => s.Category)
                .Select(g => new StockByCategoryDto
                {
                    Category = g.Key,
                    StockUnits = g.Sum(s => s.RemainStock)
                })
                .ToListAsync();
        }

        
        public async Task<List<TurnoverRatioDto>> GetMonthlyTurnoverAsync(int year, string? category = null, long? drugTypeId = null)
        {
            var months = new[] { "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var result = new List<TurnoverRatioDto>();

            // Get average inventory value once (for the year)
            var totalStockValue = await GetTotalStockValueAsync(category, drugTypeId);
            var avgInventoryValue = totalStockValue / 2m;

            for (int m = 1; m <= 12; m++)
            {
                var start = new DateTime(year, m, 1);
                var end = start.AddMonths(1).AddDays(-1);

                // Calculate COGS for the month
                var cogs = await _context.SalesInvoiceDetails
                    .Include(sid => sid.PurchaseDetail)
                    .Include(sid => sid.Drug)
                    .Where(sid => sid.SalesInvoice!.BillDate >= start && sid.SalesInvoice.BillDate <= end)
                    .Where(sid => sid.IsActive == true && sid.IsDeleted != true)
                    .Where(sid => drugTypeId == null || sid.Drug!.DrugTypeId == drugTypeId)
                    .Where(sid => string.IsNullOrEmpty(category) || sid.Drug!.DrugTypeMaster!.DrugTypeName == category)
                    .SumAsync(sid => (sid.Qty ?? 0) * (sid.PurchaseDetail!.UnitCost ?? 0m));

                var ratio = avgInventoryValue > 0 ? cogs / avgInventoryValue : 0m;
                result.Add(new TurnoverRatioDto
                {
                    Month = months[m],
                    Ratio = Math.Round(ratio, 4) // Show meaningful decimals
                });
            }

            return result;
        }
        public async Task<List<StockAlertDto>> GetStockAlertsAsync(string? category = null, long? drugTypeId = null)
        {
            var q = GetInventoryQuery(category, drugTypeId);
            return await q
                .Where(s => s.RemainStock == 0 ||
                           (s.MaxLevel.HasValue && s.RemainStock > s.MaxLevel.Value) ||
                           (s.MinLevel.HasValue && s.RemainStock < s.MinLevel.Value))
                .Select(s => new StockAlertDto
                {
                    Product = s.Name,
                    Category = s.Category,
                    CurrentStock = s.RemainStock,
                    Status = s.RemainStock == 0 ? "Stock-Out" :
                             s.RemainStock > (s.MaxLevel ?? 0) ? "Overstock" : "LowStock"
                })
                .ToListAsync();
        }

        
        private IQueryable<InventoryStockRaw> GetInventoryQuery(string? category = null, long? drugTypeId = null)
        {
            var query = _context.PurchaseDetails
                .Include(pd => pd.Drug!)
                    .ThenInclude(d => d!.DrugTypeMaster)  // JOIN DrugTypeMaster
                .Where(pd => pd.IsActive == true && pd.RemainStock > 0);

            if (drugTypeId.HasValue)
            {
                query = query.Where(pd => pd.Drug!.DrugTypeId == drugTypeId.Value);
            }
            else if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(pd => pd.Drug!.DrugTypeMaster!.DrugTypeName == category);
            }

            return query
                .GroupBy(pd => new
                {
                    pd.DrugId,
                    DrugName = pd.Drug!.DrugName ?? "Unknown",
                    Category = pd.Drug!.DrugTypeMaster!.DrugTypeName ?? "Uncategorized",
                    pd.Drug!.MinLevel,
                    pd.Drug!.MaxLevel
                })
                .Select(g => new InventoryStockRaw
                {
                    DrugId = g.Key.DrugId,
                    Name = g.Key.DrugName,
                    Category = g.Key.Category,
                    RemainStock = g.Sum(pd => pd.RemainStock ?? 0),
                    MinLevel = g.Key.MinLevel,
                    MaxLevel = g.Key.MaxLevel,
                    UnitCost = g.Average(pd => pd.UnitCost ?? 0m)  // Use PD cost, not DrugMaster
                });
        }

        

        public async Task<decimal> GetTotalSalesYtdAsync(DateTime ytdStart, DateTime ytdEnd, string? city, string? category) // CHANGED param
        {
            var q = GetFilteredQuery(ytdStart, ytdEnd, city, category);
            return await q.SumAsync(d => d.TotalAmount ?? 0m);
        }

        

        public async Task<decimal> GetSalesGrowthAsync(DateTime currentStart, DateTime currentEnd, DateTime prevStart, DateTime prevEnd, string? city, string? category) // CHANGED param
        {
            var currentSales = await GetTotalSalesAsync(currentStart, currentEnd, city, category);
            var prevSales = await GetTotalSalesAsync(prevStart, prevEnd, city, category);
            return CalculateGrowth(currentSales, prevSales);
        }

        

        public async Task<int> GetActivePharmaciesCountAsync(string? city, string? search) // CHANGED param
        {
            var q = _context.Customers.Where(c => c.BitIsActive == true && c.BitIsDelete != true);
            if (!string.IsNullOrEmpty(city)) q = q.Where(c => c.City == city); // CHANGED to City
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

       
        public async Task<decimal> GetOrderFulfillmentAsync(DateTime start, DateTime end, string? city, string? category) // CHANGED param
        {
            var totalOrders = await GetOrderCountAsync(start, end, city, category);
            var fulfilled = await _context.SalesInvoices
                .Include(si => si.Customer)
                .Where(i => i.BillDate >= start && i.BillDate <= end && i.PaymentStatus == "Paid")
                .Where(i => string.IsNullOrEmpty(city) || i.Customer!.City == city)
                .CountAsync();
            return totalOrders > 0 ? (decimal)fulfilled / totalOrders * 100 : 0m;
        }

       

        public async Task<decimal> GetFulfillmentChangeAsync(DateTime currentStart, DateTime currentEnd, DateTime prevStart, DateTime prevEnd, string? city, string? category) // CHANGED param
        {
            var current = await GetOrderFulfillmentAsync(currentStart, currentEnd, city, category);
            var prev = await GetOrderFulfillmentAsync(prevStart, prevEnd, city, category);
            return current - prev;
        }

        

        public async Task<List<MonthlyRevenueDto>> GetRevenuePerformanceAsync(int year, string? city, string? category) // CHANGED param
        {
            var raw = await GetMonthlySalesAsync(year, city, category);
            return raw.Select(r => new MonthlyRevenueDto
            {
                Month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(r.Month),
                Retail = r.Retail,
                Wholesale = r.Wholesale
            }).ToList();
        }

        
        public async Task<List<ProductCategoryShareDto>> GetProductCategoriesShareAsync(DateTime start, DateTime end, string? category)
        {
            var q = GetFilteredQuery(start, end, null, category);
            var total = await q.SumAsync(d => d.TotalAmount ?? 0m);
            return await q
                .GroupBy(d => d.Drug!.DrugTypeMaster!.DrugTypeName ?? "Unknown") // CHANGED to DrugTypeName
                .Select(g => new ProductCategoryShareDto
                {
                    Category = g.Key,
                    Share = total > 0 ? g.Sum(d => d.TotalAmount ?? 0m) / total * 100 : 0m
                })
                .ToListAsync();
        }

       

        public async Task<List<RegionalRevenueDto>> GetRegionalPerformanceAsync(DateTime start, DateTime end, string? city, string? category) // CHANGED param
        {
            var q = GetFilteredQuery(start, end, city, category);
            return await q
                .GroupBy(d => d.SalesInvoice!.Customer!.City ?? "Unknown") // CHANGED to City
                .Select(g => new RegionalRevenueDto
                {
                    Region = g.Key,
                    Revenue = g.Sum(d => d.TotalAmount ?? 0m)
                })
                .ToListAsync();
        }

        
        public async Task<List<TopPharmacyDto>> GetTopPharmaciesAsync(
    DateTime start,
    DateTime end,
    string? city,
    string? category,
    string? search,
    int topN = 10)
        {
            var query = _context.SalesInvoiceDetails
                .Include(d => d.SalesInvoice!)
                    .ThenInclude(i => i.Customer!)
                        .ThenInclude(c => c.CustomerType)
                .Include(d => d.Drug!)
                    .ThenInclude(dm => dm.DrugTypeMaster)
                .Where(d =>
                    d.SalesInvoice != null &&
                    d.SalesInvoice.BillDate.HasValue &&
                    d.SalesInvoice.BillDate.Value.Date >= start.Date &&
                    d.SalesInvoice.BillDate.Value.Date <= end.Date &&
                    d.IsActive == true && d.IsDeleted != true &&
                    d.SalesInvoice.IsActive == true && d.SalesInvoice.IsDeleted != true &&
                    d.SalesInvoice.Customer!.CustomerType!.CusTypeName == "Pharmacy"); // Only Pharmacies

            // City filter
            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(d => d.SalesInvoice!.Customer!.City == city);

            // Category filter
            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(d => d.Drug!.DrugTypeMaster!.DrugTypeName == category);

            // Search by name
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(d =>
                    (d.SalesInvoice!.Customer!.CusFirstname + " " + d.SalesInvoice!.Customer!.CusLastname).Contains(search));

            var result = await query
                .GroupBy(d => new
                {
                    d.SalesInvoice!.CustomerId,
                    Name = (d.SalesInvoice!.Customer!.CusFirstname + " " + d.SalesInvoice!.Customer!.CusLastname).Trim(),
                    City = d.SalesInvoice!.Customer!.City ?? "Unknown"
                })
                .Select(g => new TopPharmacyDto
                {
                    Name = g.Key.Name,
                    Region = g.Key.City,
                    Revenue = g.Sum(x => x.TotalAmount ?? 0m)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(topN)
                .ToListAsync();

            return result;
        }

        public async Task<List<CityDto>> GetCitiesAsync()
        {
            return await _context.Customers
                .Where(c => !string.IsNullOrEmpty(c.City))
                .Select(c => new CityDto { CityName = c.City! })
                .Distinct()
                .ToListAsync();
        }

       
        public async Task<List<ProductDropdownDto>> GetProductsAsync(string? search)
        {
            var query = _context.DrugMasters
                .Where(d => d.BitIsActive == true && (d.BitIsDelete == null || d.BitIsDelete == false));

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(d => d.DrugName != null && d.DrugName.Contains(search));

            return await query
                .Select(d => new ProductDropdownDto
                {
                    drugId = d.DrugId,
                    drugName = d.DrugName ?? string.Empty
                })
                .ToListAsync();
        }

        public async Task<List<DrugTypeDropdownDto>> GetDrugTypesAsync()
        {
            return await _context.DrugTypeMasters
                .Where(dt => dt.BitIsActive == true &&
                             (dt.BitIsDelete == null || dt.BitIsDelete == false))
                .Select(dt => new DrugTypeDropdownDto
                {
                    DrugTypeId = dt.DrugTypeId,
                    DrugTypeName = dt.DrugTypeName ?? string.Empty
                })
                .ToListAsync();
        }

        

        //+++++++
        // Helper: Filtered query (updated for DrugTypeId)
        public IQueryable<ProductSalesRaw> GetProductFilteredQuery(DateTime start, DateTime end, long? drugTypeId = null)
        {
            return _context.SalesInvoiceDetails // Assuming join with SalesInvoiceDetail + DrugMaster
                .Where(sid => sid.DateCreated >= start && sid.DateCreated <= end) // Use BillDate or DateCreated
                .Join(_context.DrugMasters,
                    sid => sid.DrugId,
                    dm => dm.DrugId,
                    (sid, dm) => new { sid, dm })
                .Where(x => !drugTypeId.HasValue || x.dm.DrugTypeId == drugTypeId.Value)
                .Where(x => x.dm.BitIsActive == true && (!x.dm.BitIsDelete.HasValue || !x.dm.BitIsDelete.Value))
                .GroupBy(x => new { x.dm.DrugId, x.dm.DrugName, x.dm.TheRapeuticclass, x.dm.DateCreated })
                .Select(g => new ProductSalesRaw
                {
                    DrugId = g.Key.DrugId,
                    Name = g.Key.DrugName ?? string.Empty,
                    Category = g.Key.TheRapeuticclass ?? string.Empty, // Or join DrugTypeMaster for Name
                    Revenue = g.Sum(x => x.sid.TotalAmount ?? 0),
                    DateCreated = g.Key.DateCreated
                });
        }

        // Helper: List version
        public async Task<List<ProductSalesRaw>> GetProductSalesListAsync(DateTime start, DateTime end, long? drugTypeId = null)
        {
            return await GetProductFilteredQuery(start, end, drugTypeId).ToListAsync();
        }

        // Updated methods (similar pattern)
        public async Task<ProductSalesRaw> GetTopProductAsync(DateTime start, DateTime end, long? drugTypeId = null)
        {
            var q = GetProductFilteredQuery(start, end, drugTypeId);
            return await q.OrderByDescending(d => d.Revenue).FirstOrDefaultAsync() ?? new ProductSalesRaw();
        }

        public async Task<ProductSalesRaw> GetFastestGrowingProductAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, long? drugTypeId = null)
        {
            var current = await GetProductSalesListAsync(start, end, drugTypeId);
            var prev = await GetProductSalesListAsync(prevStart, prevEnd, drugTypeId);
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
                Growth = CalculateGrowth(j.c.Revenue, j.p?.Revenue ?? 0) // Add Growth prop to ProductSalesRaw if needed
            }).OrderByDescending(x => x.Growth).FirstOrDefault() ?? new ProductSalesRaw();
            joined.Revenue = joined.Growth; // For KPI
            return joined;
        }

        public async Task<ProductSalesRaw> GetSlowestMovingProductAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, long? drugTypeId = null)
        {
            var current = await GetProductSalesListAsync(start, end, drugTypeId);
            var prev = await GetProductSalesListAsync(prevStart, prevEnd, drugTypeId);
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
            joined.Revenue = joined.Growth;
            return joined;
        }

        public async Task<int> GetNewLaunchesCountAsync(DateTime start, DateTime end, long? drugTypeId = null)
        {
            var query = _context.DrugMasters
                .Where(d => d.DateCreated >= start && d.DateCreated <= end)
                .Where(d => d.BitIsActive == true && (!d.BitIsDelete.HasValue || !d.BitIsDelete.Value));

            if (drugTypeId.HasValue)
            {
                query = query.Where(d => d.DrugTypeId == drugTypeId.Value);
            }

            return await query.CountAsync();
        }

        public async Task<List<TopSkuDto>> GetTopSkusAsync(DateTime start, DateTime end, int topN, long? drugTypeId = null)
        {
            var q = GetProductFilteredQuery(start, end, drugTypeId);
            return await q
                .OrderByDescending(d => d.Revenue)
                .Take(topN)
                .Select(d => new TopSkuDto { Name = d.Name, Revenue = d.Revenue })
                .ToListAsync();
        }

        public async Task<List<ProductInsightDto>> GetProductInsightsAsync(DateTime start, DateTime end, DateTime prevStart, DateTime prevEnd, long? drugTypeId = null)
        {
            var current = await GetProductSalesListAsync(start, end, drugTypeId);
            var prev = await GetProductSalesListAsync(prevStart, prevEnd, drugTypeId);
            var insights = current.Select(c => new
            {
                c,
                p = prev.FirstOrDefault(p => p.DrugId == c.DrugId),
            }).Select(j => new ProductInsightDto
            {
                Name = j.c.Name,
                Category = j.c.Category, // This will be DrugTypeName if you join in GetProductFilteredQuery
                Revenue = j.c.Revenue,
                Growth = (j.p?.Revenue > 0 ? "+" + ((j.c.Revenue - j.p.Revenue) / j.p.Revenue * 100).ToString("F0") + "%" : "New"),
                Status = DetermineStatus(j.c.DateCreated, CalculateGrowth(j.c.Revenue, j.p?.Revenue ?? 0)) // Keep for display, even without filter
            }).ToList();

            return insights.ToList(); // No status filter anymore
        }

        public async Task<List<AiRecommendationDto>> GetAiRecommendationsAsync(DateTime start, DateTime end, long? drugTypeId = null)
        {
            var sales = await GetProductSalesListAsync(start, end, drugTypeId);
            var stocks = await GetProductStocksAsync(); // Assuming this joins PurchaseDetails for stock
            var recos = sales.Select(s => new
            {
                s,
                stock = stocks.FirstOrDefault(st => st.DrugId == s.DrugId)?.RemainStock ?? 0
            }).Select(j => new AiRecommendationDto
            {
                Product = j.s.Name,
                Stock = j.stock,
                Demand = (long)(j.s.Revenue / (j.s.Revenue > 0 ? 100 : 1)), // Simplified
                Forecast = (long)(j.s.Revenue / (j.s.Revenue > 0 ? 100 : 1) * 1.2m), // +20%
                Recommendation = j.stock < (j.s.Revenue / 100) ? "Reorder " + ((j.s.Revenue / 100) - j.stock) + " units" : "Avoid Reorder"
            }).ToList();

            return recos;
        }

        // Placeholder helpers (implement as needed)
        private string DetermineStatus(DateTime? dateCreated, decimal growth)
        {
            if (dateCreated > DateTime.UtcNow.AddMonths(-1)) return "New";
            return growth > 10 ? "FastMoving" : "SlowMoving";
        }

        private decimal CalculateGrowth(decimal current, decimal previous)
        {
            return previous > 0 ? (current - previous) / previous * 100 : 0;
        }

        public async Task<List<ProductStockRaw>> GetProductStocksAsync()
        {
            // Example: Aggregate from PurchaseDetails or Sales
            return await _context.PurchaseDetails
                .Where(pd => pd.IsActive == true && pd.RemainStock > 0)
                .GroupBy(pd => pd.DrugId)
                .Select(g => new ProductStockRaw { DrugId = g.Key, RemainStock = g.Sum(x => x.RemainStock ?? 0) })
                .ToListAsync();
        }

       

        public async Task<int> GetDataYearAsync()
        {
            try
            {
                var latestBillDate = await _context.SalesInvoices
                    .Where(i => i.BillDate.HasValue)
                    .MaxAsync(i => i.BillDate); // Gets the latest DateTime?

                return latestBillDate?.Year ?? DateTime.UtcNow.Year;
            }
            catch
            {
                // If no data or error, fallback to current year
                return DateTime.UtcNow.Year;
            }
        }
    }
}
