using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkLogPro.Models;

namespace WorkLogPro.ViewModels;

// Used for the Create/Edit Page to show users a form to enter data

public class WorkItemFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Priority? Priority { get; set; } 

    [Required]
    public WorkStatus? Status { get; set; } 

    [Required]
    public DateOnly? DueDate { get; set; }
    public bool NeedsFollowUp { get; set; } 

    [Required]
    public int? CategoryId { get; set; }
    public List<SelectListItem> Categories { get; set; } = [];
}