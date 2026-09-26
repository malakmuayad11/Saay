export interface UpdateGoalDto {
  goalId: number;
  goalCategoryId: number;
  title: string;
  timeFrame: number;
  deadline: string;
  isDone: boolean;
}
