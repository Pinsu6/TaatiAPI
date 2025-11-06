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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ProductResponseDto>>>> GetProducts(
            [FromQuery] ProductFilterRequestDto request)
        {
            var response = await _productService.GetProductsAsync(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDetailDto>>> GetProductById(long id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (!response.Success)
                return NotFound(response);  // 404 if not found

            return Ok(response);
        }
    }
}
