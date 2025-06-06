using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Project
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int? CustomersId { get; set; }

    public virtual Customer? Customers { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
