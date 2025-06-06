using ConstructionManagement.Models;
using System;
using System.Windows;

namespace ConstructionManagement
{
    public partial class MainMenuWindow : Window
    {
        public Employee CurrentUser { get; set; } // Текущий пользователь

        public MainMenuWindow(Employee currentUser)
        {
            InitializeComponent();
            CurrentUser = currentUser;

            // Устанавливаем приветствие
            WelcomeLabel.Content = $"Добро пожаловать, {CurrentUser.FirstName} {CurrentUser.LastName}!";

            // Настраиваем видимость кнопок в зависимости от должности
            SetButtonVisibility();
        }

        private void SetButtonVisibility()
        {
            // По умолчанию все кнопки неактивны
            ProjectsButton.IsEnabled = false;
            CustomersButton.IsEnabled = false;
            SuppliersButton.IsEnabled = false;
            EquipmentButton.IsEnabled = false;
            MaterialsButton.IsEnabled = false;
            PaymentsButton.IsEnabled = false;
            ContractsButton.IsEnabled = false;
            TasksButton.IsEnabled = false;
            ReportsButton.IsEnabled = false;

            // Настраиваем доступность кнопок в зависимости от должности
            switch (CurrentUser.Position)
            {
                case "Инженер-строитель":
                    ProjectsButton.IsEnabled = true;
                    TasksButton.IsEnabled = true;
                    break;

                case "Архитектор":
                    ProjectsButton.IsEnabled = true;
                    TasksButton.IsEnabled = true;
                    break;

                case "Прораб":
                    ProjectsButton.IsEnabled = true;
                    TasksButton.IsEnabled = true;
                    EquipmentButton.IsEnabled = true;
                    MaterialsButton.IsEnabled = true;
                    break;

                case "Бухгалтер":
                    PaymentsButton.IsEnabled = true;
                    ContractsButton.IsEnabled = true;
                    ReportsButton.IsEnabled = true;
                    break;

                case "Менеджер по закупкам":
                    SuppliersButton.IsEnabled = true;
                    EquipmentButton.IsEnabled = true;
                    MaterialsButton.IsEnabled = true;
                    break;

                default:
                    // Если должность не определена, делаем все кнопки активными (для тестирования)
                    ProjectsButton.IsEnabled = true;
                    CustomersButton.IsEnabled = true;
                    SuppliersButton.IsEnabled = true;
                    EquipmentButton.IsEnabled = true;
                    MaterialsButton.IsEnabled = true;
                    PaymentsButton.IsEnabled = true;
                    ContractsButton.IsEnabled = true;
                    TasksButton.IsEnabled = true;
                    ReportsButton.IsEnabled = true;
                    break;
            }
        }

        // Обработчики нажатия кнопок
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
    }
}