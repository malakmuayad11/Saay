namespace Saay.Data.Entities;

public class Challenge
{
    public int ChallengeId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}