using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.IRepositories;
using TatiPharma.Application.DTOs;
using TatiPharma.Domain.Entities;
using TatiPharma.Infrastructure.Data;
using TatiPharma.Application.DTOs;

namespace TatiPharma.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<DrugMaster>> GetPagedAsync(ProductFilterRequestDto request)
        {
            var query = _context.DrugMasters
                .Include(d => d.DrugTypeMaster)
                .Include(d => d.DosageForm)
                .Include(d => d.Manufacturer)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(d =>
                    (d.DrugName != null && d.DrugName.ToLower().Contains(search)) ||
                    (d.DrugCode != null && d.DrugCode.ToLower().Contains(search)) ||
                    (d.DrugQuickcode != null && d.DrugQuickcode.ToLower().Contains(search)) ||
                    (d.DrugNdccode != null && d.DrugNdccode.ToLower().Contains(search))
                );
            }

            // Filters
            if (request.IsActive.HasValue)
                query = query.Where(d => d.BitIsActive == request.IsActive.Value);

            if (request.DrugTypeId.HasValue)
                query = query.Where(d => d.DrugTypeId == request.DrugTypeId.Value);

            if (request.DosageId.HasValue)
                query = query.Where(d => d.DosageId == request.DosageId.Value);

            if (request.ManufacturerId.HasValue)
                query = query.Where(d => d.ManufacturerId == request.ManufacturerId.Value);

            // Soft-delete
            query = query.Where(d => !d.BitIsDelete.HasValue || !d.BitIsDelete.Value);

            var totalCount = await query.CountAsync();

            var skip = (request.PageNumber - 1) * request.PageSize;
            var data = await query
                .OrderBy(d => d.DrugName)
                .Skip(skip)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<DrugMaster>
            {
                Data = data,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<DrugMaster?> GetByIdAsync(long id)
        {
            return await _context.DrugMasters
                .Include(d => d.DrugTypeMaster)
                .Include(d => d.DosageForm)
                .Include(d => d.Manufacturer)
                .FirstOrDefaultAsync(d =>
                    d.DrugId == id &&
                    (!d.BitIsDelete.HasValue || !d.BitIsDelete.Value));
        }

        //public async Task<StockSummaryDto?> GetStockSummaryAsync(long drugId)
        //{
        //    var summary = await _context.PurchaseDetails
        //        .Where(pd => pd.DrugId == drugId && (pd.IsActive ?? false) == true) // ← IMPROVED: Nullable safety
        //        .GroupBy(pd => 1)
        //        .Select(g => new StockSummaryDto
        //        {
        //            TotalPurchased = g.Sum(x => x.PurchaseStock ?? 0),
        //            TotalSold = g.Sum(x => x.SaleStock ?? 0),
        //            MinLevel = 0 // ← Default; set real value in Service
        //        })
        //        .FirstOrDefaultAsync();

        //    return summary;
        //}

        // Existing GetByIdAsync is fine (includes navigations).

        public async Task<StockSummaryDto?> GetStockSummaryAsync(long drugId)
        {
            var purchased = await _context.PurchaseDetails
                .Where(pd => pd.DrugId == drugId && pd.IsActive == true)
                .SumAsync(pd => pd.PurchaseStock ?? 0);

            var sold = await _context.SalesInvoiceDetails
                .Where(sid => sid.DrugId == drugId && sid.IsActive == true)
                .SumAsync(sid => sid.Qty ?? 0);

            var minLevel = await _context.DrugMasters
                .Where(d => d.DrugId == drugId)
                .Select(d => d.MinLevel ?? 0)
                .FirstOrDefaultAsync();

            var maxLevel = await _context.DrugMasters
                .Where(d => d.DrugId == drugId)
                .Select(d => d.MaxLevel ?? 0)
                .FirstOrDefaultAsync();

            return new StockSummaryDto
            {
                TotalPurchased = purchased,
                TotalSold = sold,
                CurrentStock = purchased - sold,
                MinLevel = minLevel,
                MaxLevel = maxLevel
                // IsLowStock/OutOfStock computed in DTO getter
            };
        }

        public async Task<List<BatchSummaryDto>> GetActiveBatchesAsync(long drugId)
        {
            return await _context.PurchaseDetails
                .Where(pd => pd.DrugId == drugId && pd.IsActive == true && (pd.RemainStock ?? 0) > 0 && pd.ExpireDate > DateTime.Today)
                .OrderBy(pd => pd.ExpireDate)
                .Select(pd => new BatchSummaryDto
                {
                    BatchNo = pd.BatchNo ?? string.Empty,
                    ExpiryDate = pd.ExpireDate,
                    RemainingQty = pd.RemainStock ?? 0
                    // IsExpiringSoon computed in DTO getter
                })
                .Take(5) // As per original
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync(long drugId)
        {
            return await _context.SalesInvoiceDetails
                .Where(sid => sid.DrugId == drugId && sid.IsActive == true)
                .SumAsync(sid => sid.TotalAmount ?? 0m);
        }

        public async Task<decimal> GetTurnoverRateAsync(long drugId)
        {
            var soldLastYear = await _context.SalesInvoiceDetails
                .Where(sid => sid.DrugId == drugId && sid.IsActive == true && sid.DateCreated > DateTime.Today.AddYears(-1))
                .SumAsync(sid => sid.Qty ?? 0);

            return soldLastYear / 12m; // Avg monthly sold (simple turnover)
        }

        public async Task<List<MonthlySalesDto>> GetMonthlySalesTrendAsync(long drugId)
        {
            var oneYearAgo = DateTime.Today.AddYears(-1);
            var sales = await _context.SalesInvoiceDetails
                .Where(sid => sid.DrugId == drugId && sid.IsActive == true && sid.DateCreated >= oneYearAgo)
                .GroupBy(sid => sid.DateCreated!.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Amount = g.Sum(sid => sid.TotalAmount ?? 0m)
                })
                .ToListAsync();

            return Enumerable.Range(1, 12)
                .Select(m => new MonthlySalesDto
                {
                    Month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m),
                    Amount = sales.FirstOrDefault(s => s.Month == m)?.Amount ?? 0m
                })
                .ToList();
        }

        public async Task<List<RegionalSalesDto>> GetRegionalSalesAsync(long drugId)
        {
            return await _context.SalesInvoiceDetails
                .Include(sid => sid.SalesInvoice!)
                    .ThenInclude(si => si.Customer)
                .Where(sid => sid.DrugId == drugId && sid.IsActive == true)
                .GroupBy(sid => sid.SalesInvoice!.Customer!.Region ?? "Unknown")
                .Select(g => new RegionalSalesDto
                {
                    Region = g.Key,
                    Amount = g.Sum(sid => sid.TotalAmount ?? 0m)
                })
                .ToListAsync();
        }

        public async Task<List<RecentOrderDto>> GetRecentOrdersAsync(long drugId)
        {
            return await _context.SalesInvoiceDetails
                .Include(sid => sid.SalesInvoice!)
                    .ThenInclude(si => si.Customer)
                .Where(sid => sid.DrugId == drugId && sid.IsActive == true)
                .OrderByDescending(sid => sid.DateCreated)
                .Select(sid => new RecentOrderDto
                {
                    OrderId = sid.SalesInvoiceId ?? 0,
                    Date = sid.SalesInvoice!.BillDate,
                    Customer = sid.SalesInvoice.Customer!.CusFirstname + " " + sid.SalesInvoice.Customer.CusLastname ?? "Unknown",
                    Quantity = sid.Qty ?? 0,
                    Total = sid.SalesInvoice.TotalAmount ?? 0m
                })
                .Take(50) // Limit to recent
                .ToListAsync();
        }

        public async Task<List<StockMovementDto>> GetStockMovementsAsync(long drugId)
        {
            var purchases = await _context.PurchaseDetails
                .Where(pd => pd.DrugId == drugId)
                .Select(pd => new StockMovementDto
                {
                    Date = pd.DateCreated,
                    Description = $"Purchased {pd.UnitQty ?? 0} units (Batch: {pd.BatchNo ?? "N/A"})"
                })
                .ToListAsync();

            var sales = await _context.SalesInvoiceDetails
                .Where(sid => sid.DrugId == drugId)
                .Select(sid => new StockMovementDto
                {
                    Date = sid.DateCreated,
                    Description = $"Sold {sid.Qty ?? 0} units (Order: {sid.SalesInvoiceId ?? 0})"
                })
                .ToListAsync();

            return purchases.Concat(sales)
                .OrderByDescending(m => m.Date)
                .Take(20) // Recent movements
                .ToList();
        }

        //public async Task<List<BatchSummaryDto>> GetActiveBatchesAsync(long drugId)
        //{
        //    return await _context.PurchaseDetails
        //        .Where(pd =>
        //            pd.DrugId == drugId &&
        //            (pd.IsActive ?? false) == true && // ← IMPROVED: Nullable safety
        //            (pd.RemainStock ?? 0) > 0 &&
        //            pd.ExpireDate > DateTime.Today)
        //        .OrderBy(pd => pd.ExpireDate)
        //        .Select(pd => new BatchSummaryDto
        //        {
        //            BatchNo = pd.BatchNo ?? string.Empty,
        //            ExpiryDate = pd.ExpireDate,
        //            RemainingQty = pd.RemainStock ?? 0
        //        })
        //        .Take(5)
        //        .ToListAsync();
        //}
    }
}
