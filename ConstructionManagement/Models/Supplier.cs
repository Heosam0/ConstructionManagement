using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public string? OrganizationName { get; set; }

    public string? Email { get; set; }

    public decimal? Phone { get; set; }

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
