using Microsoft.AspNetCore.Mvc.Rendering;
using WorkLogPro.Models;

namespace WorkLogPro.ViewModels;

// What does the user want to see? What filters can they do? One at a time or multiple

public class WorkItemListViewModel
{
    public List<WorkItem> WorkItems { get; set; } = [];

    public int TotalItems { get; set; }
    public int CompletedItems { get; set; }
    public int PendingItems { get; set; }
    public int UrgentItems { get; set; }

    public WorkStatus? SelectedStatus { get; set; }
    public int? SelectedCategoryId { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
    public bool FollowUpOnly { get; set; }

    public List<SelectListItem> Categories { get; set; } = [];
}