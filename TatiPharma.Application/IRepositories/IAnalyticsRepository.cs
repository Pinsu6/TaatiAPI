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
    }
}
