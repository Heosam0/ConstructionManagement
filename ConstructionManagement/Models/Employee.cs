using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Patronymic { get; set; }

    public string? Position { get; set; }

    public decimal? Salary { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
