using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? PlannedEndDate { get; set; }

    public DateTime? ActualEndDate { get; set; }

    public decimal? Budget { get; set; }

    public decimal? ActualCost { get; set; }

    public decimal? Progress { get; set; }

    public int? ManagerId { get; set; }

    public string? ClientName { get; set; }

    public string? ClientContact { get; set; }

    public string? Location { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<EquipmentAssignment> EquipmentAssignments { get; set; } = new List<EquipmentAssignment>();

    public virtual User? Manager { get; set; }

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
