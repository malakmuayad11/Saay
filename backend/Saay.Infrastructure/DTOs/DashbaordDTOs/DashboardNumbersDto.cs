using Saay.Infrastructure.DTOs.GoalCategoryDTOs;
using Saay.Infrastructure.DTOs.TaskDTOs;

namespace Saay.Infrastructure.DTOs.DashbaordDTOs
{
    public class DashboardNumbersDto
    {
        public string Mission { get; set; }
        public byte TodayProgress { get; set; }
        public byte TodayTasksCount { get; set; }
        public byte TodayCompletedTasksCount { get; set; }
        public byte TodayUrgentTasksCount { get; set; }
        public IEnumerable<TaskDto> TodayMainTasks { get; set; }
        public IEnumerable<GoalCategoryCountsDto> GoalCategoryCounts { get; set; }
    }
}
