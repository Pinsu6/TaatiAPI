using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TatiPharma.Application.DTOs;
using TatiPharma.Application.IServices;

namespace TatiPharma.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        private readonly IHelperService _helperService;

        public HelperController(IHelperService helperService)
        {
            _helperService = helperService;
        }

        [HttpGet("drug-types")]
        public async Task<ActionResult<ApiResponse<List<DrugTypeDropdownDto>>>> GetDrugTypes()
        {
            var response = await _helperService.GetDrugTypesForDropdownAsync();
            return Ok(response);
        }

        [HttpGet("cities")]
        public async Task<ActionResult<ApiResponse<List<CityDropdownDto>>>> GetCities()
        {
            var response = await _helperService.GetCitiesForDropdownAsync();
            return Ok(response);
        }

        [HttpGet("products")]
        public async Task<ActionResult<ApiResponse<List<ProductDropdownDto>>>> GetProducts()
        {
            var response = await _helperService.GetProductsForDropdownAsync();
            return Ok(response);
        }
    }
}
