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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PagedResult<CustomerResponseDto>>> GetCustomersAsync(CustomerFilterRequestDto request)
        {
            var pagedResult = await _customerRepository.GetPagedAsync(request);  // Pass DTO directly
            var customerDtos = _mapper.Map<List<CustomerResponseDto>>(pagedResult.Data);
            var result = new PagedResult<CustomerResponseDto>
            {
                Data = customerDtos,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };
            return ApiResponse<PagedResult<CustomerResponseDto>>.SuccessResult(result, "Customers retrieved successfully");
        }

        //public async Task<ApiResponse<CustomerDetailDto>> GetCustomerByIdAsync(long id)
        //{
        //    var customer = await _customerRepository.GetByIdAsync(id);
        //    if (customer == null)
        //    {
        //        return ApiResponse<CustomerDetailDto>.ErrorResult(new List<string> { "Customer not found" });
        //    }

        //    var dto = _mapper.Map<CustomerDetailDto>(customer);
        //    return ApiResponse<CustomerDetailDto>.SuccessResult(dto, "Customer retrieved successfully");
        //}

        public async Task<ApiResponse<CustomerDetailDto>> GetCustomerByIdAsync(long id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return ApiResponse<CustomerDetailDto>.ErrorResult(new List<string> { "Customer not found" });
            }

            var dto = _mapper.Map<CustomerDetailDto>(customer);

            // Compute KPIs
            var invoices = customer.SalesInvoices ?? new List<SalesInvoice>();
            dto.TotalOrders = invoices.Count;
            dto.LifetimeValue = invoices.Sum(i => i.TotalAmount ?? 0m);
            dto.LastPurchase = invoices.Max(i => i.BillDate);
            dto.ActivePolicies = (customer.LicenseExpiry > DateTime.UtcNow) ? 1 : 0; // Assuming one license per customer

            // Order History
            dto.OrderHistory = invoices.Select(i => new OrderHistoryDto
            {
                OrderId = i.SalesInvoiceId,
                Date = i.BillDate,
                Items = i.Details?.Count ?? 0,
                Total = i.TotalAmount ?? 0m,
                Status = i.PaymentStatus ?? "Unknown"
            }).ToList();

            // Purchase Trend (last 12 months)
            var oneYearAgo = DateTime.UtcNow.AddYears(-1);
            var monthlyGroups = invoices
                .Where(i => i.BillDate >= oneYearAgo)
                .GroupBy(i => i.BillDate?.ToString("MMM") ?? "Unknown")
                .Select(g => new MonthlyPurchaseDto
                {
                    Month = g.Key,
                    Amount = g.Sum(i => i.TotalAmount ?? 0m)
                }).ToList();
            dto.PurchaseTrend = monthlyGroups;

            // Category Split
            var allDetails = invoices.SelectMany(i => i.Details ?? new List<SalesInvoiceDetail>());
            dto.CategorySplit = allDetails
                .GroupBy(d => d.Drug?.DrugTypeMaster?.DrugTypeName ?? "Unknown")
                .Select(g => new CategorySplitDto
                {
                    Category = g.Key,
                    Amount = g.Sum(d => d.TotalAmount ?? 0m)
                }).ToList();

            // Engagement (based on purchases)
            dto.Engagement = invoices.Select(i => new EngagementDto
            {
                Type = "Purchase",
                Date = i.BillDate,
                Status = i.PaymentStatus ?? "Unknown"
            }).ToList();

            return ApiResponse<CustomerDetailDto>.SuccessResult(dto);
        }

        public async Task<PagedResult<CustomerResponseDto>> GetExportDataAsync(CustomerFilterRequestDto filter)
        {
            // KEEP THIS for full export (all records matching filters)
            filter.PageSize = int.MaxValue;
            filter.PageNumber = 1;

            var pagedResult = await _customerRepository.GetPagedAsync(filter);
            var customerDtos = _mapper.Map<List<CustomerResponseDto>>(pagedResult.Data);
            var result = new PagedResult<CustomerResponseDto>
            {
                Data = customerDtos,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };
            return result;
        }
    }
}
