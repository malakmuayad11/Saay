export interface TaskDto {
  taskId: number;
  taskCategoryTitle: string;
  title: string;
  isDone: boolean;
  dueDate: string;
  dueTime: string | null;
}
