using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;
using TatiPharma.Application.IRepositories;
using TatiPharma.Application.IServices;

namespace TatiPharma.Application.Services
{
    public class HelperService : IHelperService
    {
        private readonly IHelperRepository _helperRepository;
        private readonly IMapper _mapper;

        public HelperService(IHelperRepository helperRepository, IMapper mapper)
        {
            _helperRepository = helperRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<DrugTypeDropdownDto>>> GetDrugTypesForDropdownAsync()
        {
            var drugTypes = await _helperRepository.GetActiveDrugTypesAsync();
            var dtos = _mapper.Map<List<DrugTypeDropdownDto>>(drugTypes);

            return ApiResponse<List<DrugTypeDropdownDto>>.SuccessResult(
                dtos, "Drug types retrieved successfully");
        }
    }
}
