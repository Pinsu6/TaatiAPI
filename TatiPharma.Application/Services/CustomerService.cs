using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;
using TatiPharma.Application.IServices;
using TatiPharma.Application.IRepositories;

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

        public async Task<ApiResponse<CustomerDetailDto>> GetCustomerByIdAsync(long id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return ApiResponse<CustomerDetailDto>.ErrorResult(new List<string> { "Customer not found" });
            }

            var dto = _mapper.Map<CustomerDetailDto>(customer);
            return ApiResponse<CustomerDetailDto>.SuccessResult(dto, "Customer retrieved successfully");
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
