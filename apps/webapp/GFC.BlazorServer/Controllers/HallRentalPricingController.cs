// Hall Rental Pricing API Controller
using Microsoft.AspNetCore.Mvc;
using GFC.Core.Interfaces;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/hall-rental")]
    public class HallRentalPricingController : ControllerBase
    {
        private readonly IWebsiteSettingsService _settingsService;

        public HallRentalPricingController(IWebsiteSettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("pricing")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> GetPricing()
        {
            try
            {
                var settings = await _settingsService.GetWebsiteSettingsAsync();
                
                if (settings == null)
                {
                    return Ok(new
                    {
                        // Default fallback values
                        functionHallMemberRate = 300,
                        functionHallNonMemberRate = 400,
                        coalitionMemberRate = 100,
                        coalitionNonMemberRate = 200,
                        youthOrganizationMemberRate = 100,
                        youthOrganizationNonMemberRate = 100,
                        bartenderServiceFee = 100,
                        kitchenFee = 50,
                        avEquipmentFee = 25,
                        securityDepositAmount = 100,
                        baseFunctionHours = 5,
                        additionalHourRate = 50
                    });
                }

                return Ok(new
                {
                    functionHallMemberRate = settings.FunctionHallMemberRate ?? 300,
                    functionHallNonMemberRate = settings.FunctionHallNonMemberRate ?? 400,
                    coalitionMemberRate = settings.CoalitionMemberRate ?? 100,
                    coalitionNonMemberRate = settings.CoalitionNonMemberRate ?? 200,
                    youthOrganizationMemberRate = settings.YouthOrganizationMemberRate ?? 100,
                    youthOrganizationNonMemberRate = settings.YouthOrganizationNonMemberRate ?? 100,
                    bartenderServiceFee = settings.BartenderServiceFee ?? 100,
                    kitchenFee = settings.KitchenFee ?? 50,
                    avEquipmentFee = settings.AvEquipmentFee ?? 25,
                    securityDepositAmount = settings.SecurityDepositAmount ?? 100,
                    baseFunctionHours = settings.BaseFunctionHours ?? 5,
                    additionalHourRate = settings.AdditionalHourRate ?? 50
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching pricing", details = ex.Message });
            }
        }
    }
}
