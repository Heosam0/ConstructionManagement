using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Task
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int? ProjectId { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Project? Project { get; set; }
}
