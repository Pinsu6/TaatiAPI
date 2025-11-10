using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;
using TatiPharma.Domain.Entities;

namespace TatiPharma.Application.IRepositories
{
    public interface IProductRepository
    {
        Task<PagedResult<DrugMaster>> GetPagedAsync(ProductFilterRequestDto request);
        Task<DrugMaster?> GetByIdAsync(long id);
        Task<StockSummaryDto?> GetStockSummaryAsync(long drugId);
        Task<List<BatchSummaryDto>> GetActiveBatchesAsync(long drugId);
        Task<decimal> GetTotalRevenueAsync(long drugId);
        Task<decimal> GetTurnoverRateAsync(long drugId);
        Task<List<MonthlySalesDto>> GetMonthlySalesTrendAsync(long drugId);
        Task<List<RegionalSalesDto>> GetRegionalSalesAsync(long drugId);
        Task<List<RecentOrderDto>> GetRecentOrdersAsync(long drugId);
        Task<List<StockMovementDto>> GetStockMovementsAsync(long drugId);
    }
}
