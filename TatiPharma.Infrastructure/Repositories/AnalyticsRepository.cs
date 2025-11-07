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
        }
}
