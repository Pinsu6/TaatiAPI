using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TatiPharma.Application.DTOs;
using TatiPharma.Application.IServices;

namespace TatiPharma.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<CustomerResponseDto>>>> GetCustomers([FromQuery] CustomerFilterRequestDto request)
        {
            var response = await _customerService.GetCustomersAsync(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CustomerDetailDto>>> GetById(long id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);  // 404 if not found
            }
            return Ok(response);
        }
    }
}
