import type { GoalCategoryCountsDto } from "../goals/GoalCategoryCountsDto";
import type { TaskDto } from "../tasks/TaskDto";

export interface DashboardNumbersDto {
  mission: string;
  todayProgress: number;
  todayTasksCount: number;
  todayCompletedTasksCount: number;
  todayUrgentTasksCount: number;
  todayMainTasks: TaskDto[];
  goalCategoryCounts: GoalCategoryCountsDto[];
}
