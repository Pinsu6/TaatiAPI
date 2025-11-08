using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;

namespace TatiPharma.Application.IServices
{
    public interface IHelperService
    {
        Task<ApiResponse<List<DrugTypeDropdownDto>>> GetDrugTypesForDropdownAsync();
        Task<ApiResponse<List<CityDropdownDto>>> GetCitiesForDropdownAsync();
        Task<ApiResponse<List<ProductDropdownDto>>> GetProductsForDropdownAsync();
    }
}
