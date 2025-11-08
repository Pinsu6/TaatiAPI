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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IExportService _exportService;

        public CustomerController(ICustomerService customerService, IExportService exportService)
        {
            _customerService = customerService;
            _exportService = exportService;
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

        [HttpPost("export/pdf")]
        public async Task<IActionResult> ExportPdf([FromBody] CustomerFilterRequestDto filter)
        {
            var pagedResult = await _customerService.GetExportDataAsync(filter); // Get all matching data
            var pdfBytes = _exportService.GeneratePdf(pagedResult.Data, "Customers Export Report");
            return File(pdfBytes, "application/pdf", $"customers-{DateTime.UtcNow:yyyy-MM-dd}.pdf");
        }

        [HttpPost("export/excel")]
        public async Task<IActionResult> ExportExcel([FromBody] CustomerFilterRequestDto filter)
        {
            var pagedResult = await _customerService.GetExportDataAsync(filter); // Get all matching data
            var excelBytes = _exportService.GenerateExcel(pagedResult.Data, "Customers");
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"customers-{DateTime.UtcNow:yyyy-MM-dd}.xlsx");
        }

        [HttpPost("export/csv")]
        public async Task<IActionResult> ExportCsv([FromBody] CustomerFilterRequestDto filter)
        {
            var pagedResult = await _customerService.GetExportDataAsync(filter); // Get all matching data
            var csvBytes = _exportService.GenerateCsv(pagedResult.Data);
            return File(csvBytes, "text/csv", $"customers-{DateTime.UtcNow:yyyy-MM-dd}.csv");
        }
    }
}
