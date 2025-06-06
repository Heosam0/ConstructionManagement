using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Task
{
    public int Id { get; set; }

    public int? ProjectId { get; set; }

    public string Description { get; set; } = null!;

    public string? Status { get; set; }

    public string? Priority { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? PlannedEndDate { get; set; }

    public DateTime? ActualEndDate { get; set; }

    public int? PlannedHours { get; set; }

    public int? ActualHours { get; set; }

    public int? AssignedTo { get; set; }

    public int? CreatedBy { get; set; }

    public string? Dependencies { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? AssignedToNavigation { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Project? Project { get; set; }
}
