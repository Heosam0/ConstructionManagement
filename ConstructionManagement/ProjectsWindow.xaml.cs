using ConstructionManagement.Data;
using ConstructionManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace ConstructionManagement
{
    public partial class ProjectsWindow : Window
    {
        public List<Customer> Customers { get; set; }
        public ProjectsWindow()
        {
            InitializeComponent();
            LoadCustomers();
            LoadProjects();
        }

        private void LoadProjects()
        {
            using (var context = new AppDbContext())
            {
                var projects = context.Projects
                    .Include(p => p.Customers) // Загружаем связанных заказчиков
                    .ToList();

                ProjectsDataGrid.ItemsSource = projects;
            }
        }
        private void LoadCustomers()
        {
            using (var context = new AppDbContext())
            {
                Customers = context.Customers.ToList();
            }

            // Устанавливаем источник данных для ComboBox
            var customerColumn = ProjectsDataGrid.Columns[5] as DataGridComboBoxColumn;
            if (customerColumn != null)
            {
                customerColumn.ItemsSource = Customers;
            }
        }

        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            var addProjectWindow = new AddEditProjectWindow();
            if (addProjectWindow.ShowDialog() == true)
            {
                LoadProjects(); // Обновляем список проектов
            }
        }

        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            var selectedProject = ProjectsDataGrid.SelectedItem as Project;
            if (selectedProject != null)
            {
                var editProjectWindow = new AddEditProjectWindow(selectedProject);
                if (editProjectWindow.ShowDialog() == true)
                {
                    LoadProjects(); // Обновляем список проектов
                }
            }
            else
            {
                MessageBox.Show("Выберите проект для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            var selectedProject = ProjectsDataGrid.SelectedItem as Project;
            if (selectedProject != null)
            {
                using (var context = new AppDbContext())
                {
                    var projectToDelete = context.Projects.Find(selectedProject.Id);
                    if (projectToDelete != null)
                    {
                        context.Projects.Remove(projectToDelete);
                        context.SaveChanges();
                        LoadProjects(); // Обновляем список проектов
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите проект для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
