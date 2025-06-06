using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class EquipmentAssignment
{
    public int EquipmentId { get; set; }

    public int ProjectId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? AssignedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? AssignedByNavigation { get; set; }

    public virtual Equipment Equipment { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
