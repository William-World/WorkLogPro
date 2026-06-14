using Microsoft.AspNetCore.Mvc;
using WorkLogPro.Models;
using WorkLogPro.Services;
using WorkLogPro.ViewModels;

namespace WorkLogPro.Controllers;

public class WorkItemsController(IWorkItemService workItemService) : Controller
{
    private readonly IWorkItemService _workItemService = workItemService;

    // Without Filtering
    
    // public async Task<IActionResult> Index()
    // {
    //     var workItems = await _workItemService.GetAllAsync();

    //     WorkItemListViewModel viewModel = new()
    //     {
    //         WorkItems = workItems,
    //         TotalItems = workItems.Count,
    //         CompletedItems = workItems.Count(w => w.Status == WorkStatus.Completed),
    //         PendingItems = workItems.Count(w => w.Status != WorkStatus.Completed),
    //         UrgentItems = workItems.Count(w => w.Priority == Priority.Urgent)
    //     };

    //     return View(viewModel);
    // }

    public async Task<IActionResult> Index(
    string? searchTerm,
    WorkStatus? selectedStatus,
    int? selectedCategoryId,
    bool followUpOnly = false)
{
    var workItems = await _workItemService.GetAllAsync();

    if (!string.IsNullOrWhiteSpace(searchTerm))
    {
        workItems = workItems
            .Where(w => w.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    if (selectedStatus.HasValue)
    {
        workItems = workItems
            .Where(w => w.Status == selectedStatus.Value)
            .ToList();
    }

    if (selectedCategoryId.HasValue)
    {
        workItems = workItems
            .Where(w => w.CategoryId == selectedCategoryId.Value)
            .ToList();
    }

    if (followUpOnly)
    {
        workItems = workItems
            .Where(w => w.NeedsFollowUp)
            .ToList();
    }

    WorkItemListViewModel viewModel = new()
    {
        WorkItems = workItems,
        TotalItems = workItems.Count,
        CompletedItems = workItems.Count(w => w.Status == WorkStatus.Completed),
        PendingItems = workItems.Count(w => w.Status != WorkStatus.Completed),
        UrgentItems = workItems.Count(w => w.Priority == Priority.Urgent),

        SearchTerm = searchTerm ?? "",
        SelectedStatus = selectedStatus,
        SelectedCategoryId = selectedCategoryId,
        FollowUpOnly = followUpOnly,
        Categories = await _workItemService.GetCategorySelectListAsync()
    };

    return View(viewModel);
}

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        WorkItemFormViewModel viewModel = new()
        {
            Categories = await _workItemService.GetCategorySelectListAsync(),
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(WorkItemFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.Categories = await _workItemService.GetCategorySelectListAsync();

            return View(viewModel);
        }

        await _workItemService.CreateAsync(viewModel);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        WorkItem? workItem = await _workItemService.GetByIdAsync(id);

        if (workItem == null)
        {
            return NotFound();
        }

        WorkItemFormViewModel viewModel = new()
        {
            Id = workItem.Id,
            Title = workItem.Title,
            Description = workItem.Description,
            Priority = workItem.Priority,
            Status = workItem.Status,
            DueDate = workItem.DueDate,
            NeedsFollowUp = workItem.NeedsFollowUp,
            CategoryId = workItem.CategoryId,
            Categories = await _workItemService.GetCategorySelectListAsync()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(WorkItemFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.Categories = await _workItemService.GetCategorySelectListAsync();

            return View(viewModel);
        }

        await _workItemService.UpdateAsync(viewModel);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        WorkItem? workItem = await _workItemService.GetByIdAsync(id);

        if (workItem == null)
        {
            return NotFound();
        }

        return View(workItem);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _workItemService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ToggleFollowUp(int id)
    {
        await _workItemService.ToggleFollowUpAsync(id);

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> MarkCompleted(int id)
    {
        await _workItemService.MarkCompletedAsync(id);

        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Details(int id)
    {
        WorkItem? workItem = await _workItemService.GetByIdAsync(id);

        if (workItem == null)
        {
            return NotFound();
        }

        return View(workItem);
    }

    public async Task<IActionResult> Dashboard()
    {
        var viewModel = await _workItemService.GetDashboardAsync();

        return View(viewModel);
    }
}