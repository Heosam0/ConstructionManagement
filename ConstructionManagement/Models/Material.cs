using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Material
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Unit { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public string? Supplier { get; set; }

    public decimal? MinimumQuantity { get; set; }

    public int? ProjectId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Project? Project { get; set; }
}
