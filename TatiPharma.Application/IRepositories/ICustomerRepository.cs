using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;
using TatiPharma.Domain.Entities;

namespace TatiPharma.Application.IRepositories
{
    public interface ICustomerRepository
    {
        Task<PagedResult<Customer>> GetPagedAsync(CustomerFilterRequestDto request);
        Task<Customer?> GetByIdAsync(long id);
    }
}
