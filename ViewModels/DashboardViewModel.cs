using WorkLogPro.Models;

namespace WorkLogPro.ViewModels;

public class DashboardViewModel
{
    public int TotalItems { get; set; }
    public int CompletedItems { get; set; }
    public int InProgressItems { get; set; }
    public int WaitingItems { get; set; }
    public int OverdueItems { get; set; }
    public int FollowUpItems { get; set; }
    public List<WorkItem> RecentItems { get; set; } = [];
}