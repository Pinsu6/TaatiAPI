using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Domain.Entities;

namespace TatiPharma.Application.IRepositories
{
    public interface IHelperRepository
    {
        Task<List<DrugTypeMaster>> GetActiveDrugTypesAsync();
        Task<List<string?>> GetDistinctActiveCitiesAsync();
        Task<List<DrugMaster>> GetActiveProductsAsync();
    }
}
