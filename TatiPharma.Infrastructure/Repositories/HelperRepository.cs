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
    }
}
