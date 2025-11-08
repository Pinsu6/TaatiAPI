using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TatiPharma.Application.IRepositories;
using TatiPharma.Domain.Entities;
using TatiPharma.Infrastructure.Data;

namespace TatiPharma.Infrastructure.Repositories
{
    public class HelperRepository : IHelperRepository
    {
        private readonly AppDbContext _context;

        public HelperRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DrugTypeMaster>> GetActiveDrugTypesAsync()
        {
            return await _context.DrugTypeMasters
                .Where(d => d.BitIsActive == true &&
                           (!d.BitIsDelete.HasValue || !d.BitIsDelete.Value))
                .OrderBy(d => d.DrugTypeName)
                .ToListAsync();
        }

        public async Task<List<string?>> GetDistinctActiveCitiesAsync()
        {
            return await _context.Customers
                .Where(c => c.BitIsActive == true &&
                           (!c.BitIsDelete.HasValue || !c.BitIsDelete.Value) &&
                           !string.IsNullOrWhiteSpace(c.City))
                .Select(c => c.City)
                .Distinct()
                .OrderBy(city => city)
                .ToListAsync();
        }

        public async Task<List<DrugMaster>> GetActiveProductsAsync()
        {
            return await _context.DrugMasters
                .Where(d => d.BitIsActive == true &&
                           (!d.BitIsDelete.HasValue || !d.BitIsDelete.Value) &&
                           !string.IsNullOrWhiteSpace(d.DrugName))
                .OrderBy(d => d.DrugName)
                .ToListAsync();
        }
    }
}
