using Microsoft.AspNetCore.Mvc.Rendering;
using WorkLogPro.Models;
using WorkLogPro.ViewModels;

namespace WorkLogPro.Services;

public interface IWorkItemService
{
    Task<List<WorkItem>> GetAllAsync();
    Task<WorkItem?> GetByIdAsync(int id);

    Task CreateAsync(WorkItemFormViewModel viewModel);
    Task UpdateAsync(WorkItemFormViewModel viewModel);
    Task DeleteAsync(int id);

    Task MarkCompletedAsync(int id);
    Task ToggleFollowUpAsync(int id);

    Task<DashboardViewModel> GetDashboardAsync();
    Task<List<SelectListItem>> GetCategorySelectListAsync();
}