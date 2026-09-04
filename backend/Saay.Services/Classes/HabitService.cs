using Saay.Data.Entities;
using Saay.Infrastructure.DTOs.HabitDTOs;
using Saay.Repository.Interfaces;
using Saay.Services.Interfaces;

namespace Saay.Services.Classes
{
    public class HabitService : IHabitService
    {
        private readonly IHabitRepository _habitRepository;
        private readonly IUserRepository _userRepository;

        public HabitService(IHabitRepository habitRepository, IUserRepository userRepository)
        {
            _habitRepository = habitRepository;
            _userRepository = userRepository;
        }

        public async Task<int?> AddHabitAsync(AddHabitDto addHabitDto)
        {
            if (!await _userRepository.DoesUserExist(addHabitDto.UserId))
                return null; // User does not exist

            Habit habitEntity = new Habit
            {
                UserId = addHabitDto.UserId,
                Title = addHabitDto.Title,
                ReasonForHabit = addHabitDto.ReasonForHabit,
                Steps = addHabitDto.Steps,
                TargetDuration = addHabitDto.TargetDuration
            };

            return await _habitRepository.AddHabitAsync(habitEntity, addHabitDto.UserId);
        }

        public async Task<List<HabitDto>> GetUserHabitsAsync(int userId, int pageNumber, int pageSize)
        {
            if (!await _userRepository.DoesUserExist(userId))
                return null; // User does not exist

            List<HabitDto> habitDtos = new List<HabitDto>();

            foreach (Habit habit in await _habitRepository.GetUserHabitsAsync(userId, pageNumber, pageSize))
            {
                habitDtos.Add(new HabitDto
                {
                    HabitId = habit.HabitId,
                    UserId = habit.UserId,
                    Title = habit.Title,
                    ReasonForHabit = habit.ReasonForHabit,
                    Steps = habit.Steps,
                    TargetDuration = habit.TargetDuration
                });
            }

            return habitDtos;
        }

        public async Task<int?> UserHabitsCountAsync(int userId)
        {
            if (!await _userRepository.DoesUserExist(userId))
                return null; // User not found

            return await _habitRepository.UserHabitsCountAsync(userId);
        }

        public async Task<bool?> UpdateHabitAsync(UpdateHabitDto updateHabitDto)
        {
            Habit habit = new Habit
            {
                HabitId = updateHabitDto.HabitId,
                Title = updateHabitDto.Title,
                ReasonForHabit = updateHabitDto.ReasonForHabit,
                Steps = updateHabitDto.Steps,
                TargetDuration = updateHabitDto.TargetDuration
            };

            return await _habitRepository.UpdateHabitAsync(updateHabitDto.HabitId, habit);
        }

        public async Task<bool?> DeleteHabitAsync(int habitId) =>
            await _habitRepository.DeleteHabitAsync(habitId);

        public async Task<int?> UserCompletedHabitsCountAsync(int userId)
        {
            if (!await _userRepository.DoesUserExist(userId))
                return null; // User not found

            return await _habitRepository.UserCompletedHabitsCount(userId);
        }

        public async Task<int?> UserPendingHabitsCountAsync(int userId)
        {
            if (!await _userRepository.DoesUserExist(userId))
                return null; // User not found

            return await _habitRepository.UserPendingHabitsCount(userId);
        }

        public async Task<HabitDto> GetHabitByIdAsync(int habitId)
        {
            Habit habit = await _habitRepository.GetHabitByIdAsync(habitId);

            if (habit == null) return null;

            return new HabitDto
            {
                HabitId = habit.HabitId,
                UserId = habit.UserId,
                Title = habit.Title,
                ReasonForHabit = habit.ReasonForHabit,
                Steps = habit.Steps,
                TargetDuration = habit.TargetDuration
            };
        }
    }
}
