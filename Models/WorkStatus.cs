using System.ComponentModel.DataAnnotations;

namespace WorkLogPro.Models;

public enum WorkStatus
{
    [Display(Name = "Not Started")]
    NotStarted,

    [Display(Name = "In Progress")]
    InProgress,
    Waiting,
    Completed
}