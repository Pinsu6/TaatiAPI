using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TatiPharma.Application.DTOs;
using TatiPharma.Application.IServices;
using TatiPharma.Application.Services;

namespace TatiPharma.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IExportService _exportService;

        public ProductsController(IProductService productService, IExportService exportService)
        {
            _productService = productService;
            _exportService = exportService;
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

        [HttpPost("export/pdf")]
        public async Task<IActionResult> ExportPdf([FromBody] ProductFilterRequestDto filter)
        {
            var data = await _productService.GetExportDataAsync(filter);
            var pdfBytes = _exportService.GeneratePdf(data.Data, "Products Export Report");
            return File(pdfBytes, "application/pdf", $"products-{DateTime.UtcNow:yyyy-MM-dd}.pdf");
        }

        [HttpPost("export/excel")]
        public async Task<IActionResult> ExportExcel([FromBody] ProductFilterRequestDto filter)
        {
            var data = await _productService.GetExportDataAsync(filter);
            var excelBytes = _exportService.GenerateExcel(data.Data, "Products");
            return File(excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"products-{DateTime.UtcNow:yyyy-MM-dd}.xlsx");
        }

        [HttpPost("export/csv")]
        public async Task<IActionResult> ExportCsv([FromBody] ProductFilterRequestDto filter)
        {
            var data = await _productService.GetExportDataAsync(filter);
            var csvBytes = _exportService.GenerateCsv(data.Data);
            return File(csvBytes, "text/csv", $"products-{DateTime.UtcNow:yyyy-MM-dd}.csv");
        }
    }
}
