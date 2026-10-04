using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Saay.Infrastructure.DTOs.DashbaordDTOs;
using Saay.Services.Interfaces;

namespace Saay.API.Controllers
{
    [Route("api/saay/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IOwnershipAuthorizationService _ownershipAuthorizationService;
        private readonly ILogger<UsersController> _logger;
        private readonly IDashboardNumbersService _dashboardNumbersService;

        public DashboardController(IOwnershipAuthorizationService ownershipAuthorizationService, ILogger<UsersController> logger, IDashboardNumbersService dashboardNumbersService)
        {
            _ownershipAuthorizationService = ownershipAuthorizationService;
            _logger = logger;
            _dashboardNumbersService = dashboardNumbersService;
        }


        [EnableRateLimiting("LightOpsLimiter")]
        [Authorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<DashboardNumbersDto>> GetUserDashbaordNumbers(int userId)
        {
            if (!await _ownershipAuthorizationService.IsOwnerAsync(User, userId))
            {
                _logger.LogWarning("User {UserId} attmpted to get user dashboard numbers without ownership.",
                   userId);
                return Forbid();
            }

            DashboardNumbersDto dashboardNumbersDto = await _dashboardNumbersService.GetUserDashbaordNumbers(userId);
            return Ok(dashboardNumbersDto);
        }
    }
}
