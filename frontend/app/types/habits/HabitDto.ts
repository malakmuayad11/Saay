export interface HabitDto {
  habitId: number;
  userId: number;
  title: string;
  reasonForHabit?: string;
  steps?: string;
  targetDuration: number;
  habitStartDate: string;
}
