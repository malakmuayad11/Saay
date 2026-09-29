namespace Saay.Infrastructure.DTOs.TaskDTOs
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public string TaskCategoryTitle { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public DateOnly DueDate { get; set; }
        public TimeOnly? DueTime { get; set; }

    }
}
