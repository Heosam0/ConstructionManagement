using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Patronymic { get; set; }

    public string? OrganizationName { get; set; }

    public string? Email { get; set; }

    public decimal? Phone { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
