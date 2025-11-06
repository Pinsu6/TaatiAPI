using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;
using TatiPharma.Domain.Entities;
using TatiPharma.Infrastructure.Data;
using TatiPharma.Application.IRepositories;

namespace TatiPharma.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Customer>> GetPagedAsync(CustomerFilterRequestDto request)
        {
            // Same logic as before (query building with request.Search, request.IsActive, etc.)
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var searchLower = request.Search.ToLower();
                query = query.Where(c =>
                    (c.CusFirstname != null && c.CusFirstname.ToLower().Contains(searchLower)) ||
                    (c.CusLastname != null && c.CusLastname.ToLower().Contains(searchLower)) ||
                    (c.CusCode != null && c.CusCode.ToLower().Contains(searchLower)) ||
                    (c.CusEmail != null && c.CusEmail.ToLower().Contains(searchLower))
                );
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(c => c.BitIsActive == request.IsActive.Value);
            }

            query = query.Where(c => !c.BitIsDelete.HasValue || !c.BitIsDelete.Value);

            var totalCount = await query.CountAsync();

            var skip = (request.PageNumber - 1) * request.PageSize;
            var data = await query
                .OrderBy(c => c.CusFirstname)
                .Skip(skip)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<Customer>
            {
                Data = data,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<Customer?> GetByIdAsync(long id)
        {
            return await _context.Customers
                .Include(c => c.CustomerType)  // Eager load relations
                .Include(c => c.Employee)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
        }
    }
}
