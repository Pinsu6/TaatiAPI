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

        public async Task<StockSummaryDto?> GetStockSummaryAsync(long drugId)
        {
            var summary = await _context.PurchaseDetails
                .Where(pd => pd.DrugId == drugId && (pd.IsActive ?? false) == true) // ← IMPROVED: Nullable safety
                .GroupBy(pd => 1)
                .Select(g => new StockSummaryDto
                {
                    TotalPurchased = g.Sum(x => x.PurchaseStock ?? 0),
                    TotalSold = g.Sum(x => x.SaleStock ?? 0),
                    MinLevel = 0 // ← Default; set real value in Service
                })
                .FirstOrDefaultAsync();

            return summary;
        }

        public async Task<List<BatchSummaryDto>> GetActiveBatchesAsync(long drugId)
        {
            return await _context.PurchaseDetails
                .Where(pd =>
                    pd.DrugId == drugId &&
                    (pd.IsActive ?? false) == true && // ← IMPROVED: Nullable safety
                    (pd.RemainStock ?? 0) > 0 &&
                    pd.ExpireDate > DateTime.Today)
                .OrderBy(pd => pd.ExpireDate)
                .Select(pd => new BatchSummaryDto
                {
                    BatchNo = pd.BatchNo ?? string.Empty,
                    ExpiryDate = pd.ExpireDate,
                    RemainingQty = pd.RemainStock ?? 0
                })
                .Take(5)
                .ToListAsync();
        }
    }
}
