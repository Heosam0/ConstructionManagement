using ConstructionManagement.Data;
using ConstructionManagement.Models;
using ConstructionManagement.Services;
using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Separator = LiveCharts.Wpf.Separator;

namespace ConstructionManagement
{
    public partial class MainMenuWindow : Window
    {
        private readonly DashboardService _dashboardService;
        private readonly Employee _currentEmployee;
        private DashboardData _dashboardData;

        public SeriesCollection ProjectSeries { get; set; }
        public SeriesCollection RevenueSeries { get; set; }
        public Func<double, string> PercentFormatter { get; set; }
        public Func<double, string> CurrencyFormatter { get; set; }

        public MainMenuWindow(Employee employee)
        {
            InitializeComponent();
            _currentEmployee = employee;
            _dashboardService = new DashboardService(new AppDbContext());

            // Инициализация форматтеров для графиков
            PercentFormatter = value => value.ToString("N1") + "%";
            CurrencyFormatter = value => value.ToString("N0") + " ₽";

            // Установка контекста данных
            DataContext = this;

            LoadDashboardData();
            UpdateUserInfo();
        }

        private async void LoadDashboardData()
        {
            try
            {
                _dashboardData = await _dashboardService.GetDashboardDataAsync();
                UpdateDashboardUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateUserInfo()
        {
            UserNameText.Text = $"{_currentEmployee.FirstName} {_currentEmployee.LastName}";
            UserRoleText.Text = _currentEmployee.Position;
        }

        private void UpdateDashboardUI()
        {
            try
            {
                // Обновляем статистику
                txtActiveProjectsCount.Text = _dashboardData.ActiveProjectsCount.ToString();
                txtTasksCount.Text = _dashboardData.PendingTasksCount.ToString();
                txtMonthlyExpenses.Text = $"{_dashboardData.MonthlyExpenses:N0} ₽";
                txtCompletedProjectsCount.Text = _dashboardData.CompletedProjectsCount.ToString();

                // Обновляем график выполнения проектов
                var projectValues = new ChartValues<double>();
                var projectLabels = new List<string>();

                foreach (var project in _dashboardData.ProjectProgress)
                {
                    projectValues.Add(project.ProgressPercentage);
                    projectLabels.Add(project.ProjectName);
                }

                ProjectSeries = new SeriesCollection
        {
            new ColumnSeries
            {
                Values = projectValues,
                Title = "Выполнение",
                Fill = (SolidColorBrush)Application.Current.Resources["PrimaryHueMidBrush"]
            }
        };

                chartProjectProgress.Series = ProjectSeries;
                chartProjectProgress.AxisX = new AxesCollection
        {
            new Axis
            {
                Labels = projectLabels,
                Separator = new Separator { Step = 1 }
            }
        };

                // Обновляем график доходов
                var revenueValues = new ChartValues<decimal>(_dashboardData.MonthlyRevenue.Values);
                var revenueLabels = _dashboardData.MonthlyRevenue.Keys.ToList();

                RevenueSeries = new SeriesCollection
        {
            new LineSeries
            {
                Values = revenueValues,
                Title = "Доходы",
                LineSmoothness = 0,
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 10,
                Stroke = (SolidColorBrush)Application.Current.Resources["SecondaryAccentBrush"]
            }
        };

                chartRevenue.Series = RevenueSeries;
                chartRevenue.AxisX = new AxesCollection
        {
            new Axis
            {
                Labels = revenueLabels,
                Separator = new Separator { Step = 1 }
            }
        };

                // Обновляем список задач
                lvTasks.Items.Clear();
                foreach (var task in _dashboardData.RecentTasks)
                {
                    lvTasks.Items.Add(new TaskViewModel
                    {
                        Description = task.Description,
                        Status = task.Status,
                        DueDate = task.DueDate,
                        StatusIcon = task.Status == "Completed" ? PackIconKind.CheckCircle : PackIconKind.CircleOutline
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении интерфейса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            var projectsWindow = new ProjectsWindow();
            projectsWindow.Show();
        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            var customersWindow = new CustomersWindow();
            customersWindow.Show();
        }

        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            var suppliersWindow = new SuppliersWindow();
            suppliersWindow.Show();
        }

        private void EquipmentButton_Click(object sender, RoutedEventArgs e)
        {
            var equipmentWindow = new EquipmentWindow();
            equipmentWindow.Show();
        }

        private void MaterialsButton_Click(object sender, RoutedEventArgs e)
        {
            var materialsWindow = new MaterialsWindow();
            materialsWindow.Show();
        }

        private void PaymentsButton_Click(object sender, RoutedEventArgs e)
        {
            var paymentsWindow = new PaymentsWindow();
            paymentsWindow.Show();
        }

        private void ContractsButton_Click(object sender, RoutedEventArgs e)
        {
            var contractsWindow = new ContractsWindow();
            contractsWindow.Show();
        }

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            var tasksWindow = new TasksWindow();
            tasksWindow.Show();
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            new ReportWindow().Show();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDashboardData();
        }
    }

    public class TaskViewModel
    {
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public PackIconKind StatusIcon { get; set; }
    }
}