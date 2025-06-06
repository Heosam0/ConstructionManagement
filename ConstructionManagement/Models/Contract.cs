using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Contract
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime DateOfConclusion { get; set; }

    public decimal? Amount { get; set; }

    public int? CustomersId { get; set; }

    public virtual Customer? Customers { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
