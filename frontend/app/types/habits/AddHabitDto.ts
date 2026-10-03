export interface AddHabitDto {
  userId: number;
  title: string;
  reasonForHabit: string;
  steps: string;
  targetDuration: number;
}
