using Saay.Infrastructure.DTOs.HabitDTOs;

namespace Saay.Services.Interfaces
{
    public interface IHabitService
    {
        public Task<int?> AddHabitAsync(AddHabitDto addHabitDto);

        public Task<List<HabitDto>> GetUserHabitsAsync(int userId, int pageNumber, int pageSize);

        public Task<int?> UserHabitsCountAsync(int userId);

        public Task<bool?> UpdateHabitAsync(UpdateHabitDto updateHabitDto);

        public Task<bool?> DeleteHabitAsync(int habitId);

        public Task<int?> UserCompletedHabitsCountAsync(int userId);

        public Task<int?> UserPendingHabitsCountAsync(int userId);

        public Task<HabitDto> GetHabitByIdAsync(int habitId);

        public Task<bool> DoesHabitExistAsync(int habitId);

        public Task<byte?> GetHabitTargetDurationAsync(int habitId);

        public Task<bool> IsHabitOwner(int userId, int habitId);
    }
}
