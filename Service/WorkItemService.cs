using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkLogPro.Data;
using WorkLogPro.Models;
using WorkLogPro.ViewModels;

namespace WorkLogPro.Services;

public class WorkItemService(AppDbContext context) : IWorkItemService
{
    private readonly AppDbContext _context = context;

    public async Task<List<WorkItem>> GetAllAsync()
    {
        // Get all the work items
        return await _context.WorkItems.ToListAsync();
    }

    public async Task<WorkItem?> GetByIdAsync(int id)
    {
        return await _context.WorkItems
            .Include(workItem => workItem.Category)
            .FirstOrDefaultAsync(workItem => workItem.Id == id);

        // Get a work item by id
        // return await _context.WorkItems
        //     .FirstOrDefaultAsync(workItem => workItem.Id == id); // teaches LINQ

        // return await _context.WorkItems.FindAsync(id); Literally saying Find the WorkItem whose primary key is this id. Use when searching for primary key
    }

    public async Task CreateAsync(WorkItemFormViewModel viewModel)
    {
        var workItem = new WorkItem
        {
            Title = viewModel.Title,
            Description = viewModel.Description,
            Priority = viewModel.Priority!.Value,
            Status = viewModel.Status!.Value,
            DueDate = viewModel.DueDate!.Value,
            NeedsFollowUp = viewModel.NeedsFollowUp,
            CategoryId = viewModel.CategoryId!.Value,

            CreatedDate = DateOnly.FromDateTime(DateTime.Now)
        };

        _context.WorkItems.Add(workItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(WorkItemFormViewModel viewModel)
    {
        WorkItem? existingWorkItem = await GetByIdAsync(viewModel.Id);

        if (existingWorkItem == null)
        {
            return;
        }

        existingWorkItem.Title = viewModel.Title;
        existingWorkItem.Description = viewModel.Description;
        existingWorkItem.Priority = viewModel.Priority!.Value;
        existingWorkItem.Status = viewModel.Status!.Value;
        existingWorkItem.DueDate = viewModel.DueDate!.Value;
        existingWorkItem.NeedsFollowUp = viewModel.NeedsFollowUp;
        existingWorkItem.CategoryId = viewModel.CategoryId!.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        WorkItem? workItem = await GetByIdAsync(id);

        if (workItem == null)
        {
            return;
        }

        _context.WorkItems.Remove(workItem);
        await _context.SaveChangesAsync();

    }

    public async Task MarkCompletedAsync(int id)
    {
        WorkItem? workItem = await GetByIdAsync(id);

        if (workItem == null)
        {
            return;
        }

        workItem.Status = WorkStatus.Completed;
        workItem.CompletedDate = DateOnly.FromDateTime(DateTime.Now);

        await _context.SaveChangesAsync();
    }

    public async Task ToggleFollowUpAsync(int id)
    {
        WorkItem? workItem = await GetByIdAsync(id);

        if (workItem == null)
        {
            return;
        }

        if (!workItem.NeedsFollowUp)
        {
            workItem.NeedsFollowUp = true;
        }
        else
        {
            workItem.NeedsFollowUp = false;
        }
        
        // or shorthand syntax -> workItem.NeedsFollowUp = !workItem.NeedsFollowUp;

        await _context.SaveChangesAsync();
    }

    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var workItems = await GetAllAsync();
        var today = DateOnly.FromDateTime(DateTime.Now);

        var dashboardViewModel = new DashboardViewModel
        {
            TotalItems = workItems.Count,

            CompletedItems = workItems.Count(workItem => 
                workItem.Status == WorkStatus.Completed),

            InProgressItems = workItems.Count(workItem => 
                workItem.Status == WorkStatus.InProgress),

            WaitingItems = workItems.Count(workItem => 
                workItem.Status == WorkStatus.Waiting),

            OverdueItems = workItems.Count(workItem =>
                workItem.DueDate < today &&
                workItem.Status != WorkStatus.Completed),

            FollowUpItems = workItems.Count(workItem =>
                workItem.NeedsFollowUp),

            RecentItems = [.. workItems
                .OrderByDescending(workItem => workItem.CreatedDate)
                .ThenByDescending(workItem => workItem.Id)
                .Take(5)]
        };
        
        return dashboardViewModel;
    }

    public async Task<List<SelectListItem>> GetCategorySelectListAsync()
    {
        var categories = await _context.Categories.ToListAsync();

        var categoryList = categories.Select(category => 
            new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.Name
            }).ToList();
        
        return categoryList;
    }
}