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
using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using ConstructionManagement.Data;

namespace ConstructionManagement
{
    public partial class TasksWindow : Window
    {
        public TasksWindow()
        {
            InitializeComponent();
            LoadTasks();
        }

        private void LoadTasks()
        {
            using (var context = new AppDbContext())
            {
                // Загружаем задачи с проектами и сотрудниками
                var tasks = context.Tasks
                    .Include(t => t.Project)
                    .Include(t => t.Employee)
                    .ToList();

                TasksDataGrid.ItemsSource = tasks;
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddEditTaskWindow();
            if (addTaskWindow.ShowDialog() == true)
            {
                LoadTasks(); // Обновляем список задач
            }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = TasksDataGrid.SelectedItem as Models.Task;
            if (selectedTask != null)
            {
                var editTaskWindow = new AddEditTaskWindow(selectedTask);
                if (editTaskWindow.ShowDialog() == true)
                {
                    LoadTasks(); // Обновляем список задач
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var selectedTask = TasksDataGrid.SelectedItem as Task;
            if (selectedTask != null)
            {
                using (var context = new AppDbContext())
                {
                    var taskToDelete = context.Tasks.Find(selectedTask.Id);
                    if (taskToDelete != null)
                    {
                        context.Tasks.Remove(taskToDelete);
                        context.SaveChanges();
                        LoadTasks(); // Обновляем список задач
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}