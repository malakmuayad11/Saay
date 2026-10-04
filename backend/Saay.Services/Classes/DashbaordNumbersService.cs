using Saay.Infrastructure.DTOs.DashbaordDTOs;
using Saay.Infrastructure.DTOs.GoalCategoryDTOs;
using Saay.Infrastructure.DTOs.TaskDTOs;
using Saay.Services.Interfaces;

namespace Saay.Services.Classes
{
    public class DashbaordNumbersService: IDashboardNumbersService
    {
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;
        private readonly IGoalCategoryService _goalCategoryService;

        public DashbaordNumbersService(IUserService userService, ITaskService taskService, IGoalCategoryService goalCategoryService)
        {
            _userService = userService;
            _taskService = taskService;
            _goalCategoryService = goalCategoryService;
        }

        public async Task<DashboardNumbersDto> GetUserDashbaordNumbers(int userId)
        {
            string mission = await _userService.GetUserMissionAsync(userId) ?? "";
            byte? todayProgress = await _taskService.UserProgressToday(userId) ?? 0;
            byte? todayTasksCount = await _taskService.UserTasksTodayCountAsync(userId);
            byte? todayCompletedTasksCount = await _taskService.UserCompletedTasksTodayCount(userId);
            byte? todayUrgentTasksCount = await _taskService.UserUrgentTasksTodayCount(userId);
            List<TaskDto> todayMainTasks = await _taskService.GetUserTasksTodayAsync(userId, 1, 3);
            List<GoalCategoryCountsDto > goalCategoryCounts = await _goalCategoryService.GetAllGoalCategoriesWithCountsAsync(userId);

            return new DashboardNumbersDto
            {
                Mission = mission ?? string.Empty,
                TodayProgress = todayProgress ?? 0,
                TodayTasksCount = todayTasksCount ?? 0,
                TodayCompletedTasksCount = todayCompletedTasksCount ?? 0,
                TodayUrgentTasksCount = todayUrgentTasksCount ?? 0,
                TodayMainTasks = todayMainTasks,
                GoalCategoryCounts = goalCategoryCounts
            };
        }
    }
}
