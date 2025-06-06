using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Equipment
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime DateOfPurchase { get; set; }

    public decimal? Cost { get; set; }

    public int? SupplierId { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
