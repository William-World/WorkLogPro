namespace WorkLogPro.Models;

public class WorkItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; } 
    public WorkStatus Status { get; set; } 
    public DateOnly DueDate { get; set; }
    public DateOnly CreatedDate { get; set; }
    public DateOnly? CompletedDate { get; set; }
    public bool NeedsFollowUp { get; set; } 
    public int CategoryId { get; set; }
    public Category? Category {get; set; }

}