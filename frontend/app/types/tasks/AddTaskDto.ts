export interface AddTaskDto {
  userId: number;
  taskCategoryId: number;
  title: string;
  repetition: number;
  dueDate: string;
  dueTime: string | null;
}
