using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? ProjectId { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = null!;

    public string? Category { get; set; }

    public string? Description { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Project? Project { get; set; }
}
