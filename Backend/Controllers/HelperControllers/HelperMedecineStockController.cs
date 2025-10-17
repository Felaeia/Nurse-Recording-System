using Microsoft.AspNetCore.Mvc;
using NurseRecordingSystem.Contracts.ServiceContracts.HelperContracts.IHelperUserForm;

[Route("api/helper/medecine")]
[ApiController]
// [Authorize] // Authorization should be applied in a real application
public class HelperMedecineStockController : ControllerBase
{
    private readonly IViewAllMedecineStocks _viewAllService;

    public HelperMedecineStockController(IViewAllMedecineStocks viewAllService)
    {
        _viewAllService = viewAllService;
    }

    [HttpGet("view_all_stock")]
    public async Task<IActionResult> ViewAllStock()
    {
        try
        {
            var stockList = await _viewAllService.ViewAllAsync();
            return Ok(stockList); // HTTP 200 OK
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}