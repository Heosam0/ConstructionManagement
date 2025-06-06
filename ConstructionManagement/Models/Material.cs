using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Material
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? Price { get; set; }

    public int? SupplierId { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
