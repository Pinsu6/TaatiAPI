using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;
using TatiPharma.Application.IRepositories;
using TatiPharma.Application.IServices;
using TatiPharma.Domain.Entities;

namespace TatiPharma.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PagedResult<ProductResponseDto>>> GetProductsAsync(ProductFilterRequestDto request)
        {
            var pagedResult = await _productRepository.GetPagedAsync(request);

            var dtos = _mapper.Map<List<ProductResponseDto>>(pagedResult.Data);

            var result = new PagedResult<ProductResponseDto>
            {
                Data = dtos,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };

            return ApiResponse<PagedResult<ProductResponseDto>>.SuccessResult(result, "Products retrieved successfully");
        }

        //public async Task<ApiResponse<ProductDetailDto>> GetProductByIdAsync(long id)
        //{
        //    var product = await _productRepository.GetByIdAsync(id);
        //    if (product == null)
        //        return ApiResponse<ProductDetailDto>.ErrorResult(new List<string> { "Product not found" }, "Product not found");

        //    var dto = _mapper.Map<ProductDetailDto>(product);  // Maps full details + navigations
        //    return ApiResponse<ProductDetailDto>.SuccessResult(dto, "Product retrieved successfully");
        //}

        //public async Task<ApiResponse<ProductDetailDto>> GetProductByIdAsync(long id)
        //{
        //    var product = await _productRepository.GetByIdAsync(id);
        //    if (product == null)
        //        return ApiResponse<ProductDetailDto>.ErrorResult(
        //            new List<string> { "Product not found" });

        //    var dto = _mapper.Map<ProductDetailDto>(product);

        //    // === Stock Summary ===
        //    var stockSummary = await _productRepository.GetStockSummaryAsync(id);
        //    if (stockSummary != null)
        //    {
        //        stockSummary.MinLevel = product.MinLevel ?? 0; // ← FIXED: Set MinLevel; IsLowStock computes via getter
        //        dto.StockSummary = stockSummary;
        //    }

        //    // === Active Batches ===
        //    dto.ActiveBatches = await _productRepository.GetActiveBatchesAsync(id);

        //    // === Pricing ===
        //    dto.Pricing = new PriceBreakdownDto
        //    {
        //        UnitCost = product.UnitCost ?? 0m,
        //        MarginPercent = product.Margin ?? 0m
        //    };

        //    return ApiResponse<ProductDetailDto>.SuccessResult(dto, "Product retrieved successfully");
        //}

        public async Task<ApiResponse<ProductDetailDto>> GetProductByIdAsync(long id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return ApiResponse<ProductDetailDto>.ErrorResult(new List<string> { "Product not found" });

            var dto = _mapper.Map<ProductDetailDto>(product);

            // Stock Summary
            dto.StockSummary = await _productRepository.GetStockSummaryAsync(id);
            if (dto.StockSummary != null)
            {
                dto.StockSummary.MinLevel = product.MinLevel ?? 0;
                dto.StockSummary.MaxLevel = product.MaxLevel ?? 0;
            }

            // Active Batches
            dto.ActiveBatches = await _productRepository.GetActiveBatchesAsync(id);

            // Pricing
            dto.Pricing = new PriceBreakdownDto
            {
                UnitCost = product.UnitCost ?? 0m,
                MarginPercent = product.Margin ?? 0m,
                MarginAmount = (product.UnitCost ?? 0m) * (product.Margin ?? 0m) / 100m,
                SalePrice = (product.UnitCost ?? 0m) * (1 + (product.Margin ?? 0m) / 100m)
            };

            // Extras
            dto.TotalRevenue = await _productRepository.GetTotalRevenueAsync(id);
            dto.TurnoverRate = await _productRepository.GetTurnoverRateAsync(id);
            dto.MonthlySalesTrend = await _productRepository.GetMonthlySalesTrendAsync(id);
            dto.RegionalSales = await _productRepository.GetRegionalSalesAsync(id);
            dto.RecentOrders = await _productRepository.GetRecentOrdersAsync(id);
            dto.StockMovements = await _productRepository.GetStockMovementsAsync(id);

            // Alerts (computed)
            dto.Alerts = new List<AlertDto>();
            if (dto.StockSummary?.IsLowStock ?? false) dto.Alerts.Add(new AlertDto { Message = "Low Stock Alert" });
            if (dto.StockSummary?.IsOutOfStock ?? false) dto.Alerts.Add(new AlertDto { Message = "Out of Stock" });
            if (dto.ActiveBatches.Any(b => b.IsExpiringSoon)) dto.Alerts.Add(new AlertDto { Message = "Batch Expiring Soon" });

            return ApiResponse<ProductDetailDto>.SuccessResult(dto, "Product retrieved successfully");
        }

        public async Task<PagedResult<ProductResponseDto>> GetExportDataAsync(ProductFilterRequestDto filter)
        {
            // Fetch ALL matching records (ignore pagination)
            filter.PageSize = int.MaxValue;
            filter.PageNumber = 1;

            var pagedResult = await _productRepository.GetPagedAsync(filter);
            var dtos = _mapper.Map<List<ProductResponseDto>>(pagedResult.Data);

            return new PagedResult<ProductResponseDto>
            {
                Data = dtos,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };
        }
    }
}
