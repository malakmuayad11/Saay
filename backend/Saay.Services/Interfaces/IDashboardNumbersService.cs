using Saay.Infrastructure.DTOs.DashbaordDTOs;

namespace Saay.Services.Interfaces
{
    public interface IDashboardNumbersService
    {
        public Task<DashboardNumbersDto> GetUserDashbaordNumbers(int userId);
    }
}
