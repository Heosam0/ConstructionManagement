using ConstructionManagement.Data;
using ConstructionManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionManagement.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardData> GetDashboardDataAsync()
        {
            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);


            // Загружаем проекты с задачами заранее
            var projects = await _context.Projects
                .Where(p => p.EndDate.ToUniversalTime() >= now)
                .Include(p => p.Tasks)
                .ToListAsync();

            var projectProgress = projects.Select(p => new ProjectProgress
            {
                ProjectName = p.Name ?? "Без названия",
                EndDate = p.EndDate,
                Status = GetProjectStatus(p.Tasks),
                ProgressPercentage = GetProjectProgress(p.Tasks)
            }).ToList();

            var paymentData = await _context.Payments
                .Where(p => p.Date.ToUniversalTime() >= monthStart &&
                            p.Date.ToUniversalTime() <= monthEnd &&
                            p.Type == "Income")
                .ToListAsync(); // Сначала получаем данные

            var monthlyRevenue = paymentData
                .GroupBy(p => p.Date.ToString("dd.MM"))
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(p => p.Amount ?? 0)
                );

            var materialData = await _context.Materials.ToListAsync();
            var materialsInventory = materialData
                .GroupBy(m => m.Name)
                .ToDictionary(
                    g => g.Key ?? "Без названия",
                    g => g.Sum(m => m.Quantity ?? 0)
                );

            var dashboardData = new DashboardData
            {


                ActiveProjectsCount = await _context.Projects
                    .CountAsync(p => p.EndDate.ToUniversalTime() >= now),

                PendingTasksCount = await _context.Tasks
                    .CountAsync(t => t.Status != "Completed" && t.EndDate.ToUniversalTime() >= now),

                MonthlyExpenses = await _context.Payments
                    .Where(p => p.Date.ToUniversalTime() >= monthStart &&
                               p.Date.ToUniversalTime() <= monthEnd)
                    .SumAsync(p => p.Amount ?? 0),

                CompletedProjectsCount = await _context.Projects
                    .CountAsync(p => p.EndDate.ToUniversalTime() < now),

                

                RecentTasks = await _context.Tasks
                    .Include(t => t.Employee)
                    .Where(t => t.EndDate.ToUniversalTime() >= now)
                    .OrderBy(t => t.EndDate)
                    .Take(5)
                    .Select(t => new TaskInfo
                    {
                        Id = t.Id,
                        Description = t.Description ?? "Без описания",
                        Status = t.Status ?? "Новая",
                        DueDate = t.EndDate,
                        AssignedTo = $"{t.Employee.FirstName} {t.Employee.LastName}",
                        Priority = t.EndDate.ToUniversalTime() <= now.AddDays(3) ? "High" : "Normal"
                    })
                    .ToListAsync(),

                ProjectProgress = projectProgress,
                MonthlyRevenue = monthlyRevenue,
                MaterialsInventory = materialsInventory
            };

            // Если данных нет, добавим тестовые данные
            if (!dashboardData.MonthlyRevenue.Any())
            {
                dashboardData.MonthlyRevenue = new Dictionary<string, decimal>
        {
            { "01.06", 100000 },
            { "02.06", 150000 },
            { "03.06", 120000 },
            { "04.06", 180000 },
            { "05.06", 200000 }
        };
            }

            if (!dashboardData.ProjectProgress.Any())
            {
                dashboardData.ProjectProgress = new List<ProjectProgress>
        {
            new ProjectProgress { ProjectName = "Проект 1", ProgressPercentage = 75 },
            new ProjectProgress { ProjectName = "Проект 2", ProgressPercentage = 45 },
            new ProjectProgress { ProjectName = "Проект 3", ProgressPercentage = 90 }
        };
            }

            return dashboardData;
        }

        private string CalculateProjectStatus(ICollection<Models.Task> tasks)
        {
            if (!tasks.Any())
                return "0%";

            var completedTasks = tasks.Count(t => t.Status == "Completed");
            var totalTasks = tasks.Count;
            return ((double)completedTasks / totalTasks * 100).ToString("F1") + "%";
        }

        // Выносим методы расчета в статические
        private static string GetProjectStatus(ICollection<Models.Task> tasks)
        {
            if (tasks == null || !tasks.Any())
                return "0%";

            var completedTasks = tasks.Count(t => t.Status == "Completed");
            var totalTasks = tasks.Count;
            return ((double)completedTasks / totalTasks * 100).ToString("F1") + "%";
        }

        private static double GetProjectProgress(ICollection<Models.Task> tasks)
        {
            if (tasks == null || !tasks.Any())
                return 0;

            var completedTasks = tasks.Count(t => t.Status == "Completed");
            var totalTasks = tasks.Count;
            return (double)completedTasks / totalTasks * 100;
        }

        private double CalculateProjectProgress(ICollection<Models.Task> tasks)
        {
            if (!tasks.Any())
                return 0;

            var completedTasks = tasks.Count(t => t.Status == "Completed");
            var totalTasks = tasks.Count;
            return (double)completedTasks / totalTasks * 100;
        }
    }

}