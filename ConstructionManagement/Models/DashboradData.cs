using System;
using System.Collections.Generic;

namespace ConstructionManagement.Models
{
    public class DashboardData
    {
        public int ActiveProjectsCount { get; set; }
        public int PendingTasksCount { get; set; }
        public decimal MonthlyExpenses { get; set; }
        public int CompletedProjectsCount { get; set; }
        public List<ProjectProgress> ProjectProgress { get; set; }
        public List<TaskInfo> RecentTasks { get; set; }
        public Dictionary<string, decimal> MonthlyRevenue { get; set; }
        public Dictionary<string, decimal> MaterialsInventory { get; set; } // Изменили тип на decimal
    }

    public class ProjectProgress
    {
        public string ProjectName { get; set; }
        public double ProgressPercentage { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }

    public class TaskInfo
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public string AssignedTo { get; set; }
        public string Priority { get; set; }
    }
}