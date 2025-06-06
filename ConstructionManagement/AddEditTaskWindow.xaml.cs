using System.Windows;
using System.Windows.Controls;
using ConstructionManagement.Data;
using ConstructionManagement.Models;
using Task = ConstructionManagement.Models.Task;

namespace ConstructionManagement
{
    public partial class AddEditTaskWindow : Window
    {
        public Models.Task Task { get; private set; } // Задача для добавления или редактирования
        public bool IsEditMode { get; private set; } // Режим редактирования (true) или добавления (false)
        public List<Project> Projects { get; set; } // Список проектов
        public List<Employee> Employees { get; set; } // Список сотрудников

        public AddEditTaskWindow()
        {
            InitializeComponent();
            Task = new Task(); // Создаем новую задачу
            IsEditMode = false; // Режим добавления
            LoadProjectsAndEmployees(); // Загружаем списки проектов и сотрудников
            DataContext = this; // Устанавливаем контекст данных
        }

        public AddEditTaskWindow(Task taskToEdit)
        {
            InitializeComponent();
            Task = taskToEdit; // Редактируем существующую задачу
            IsEditMode = true; // Режим редактирования
            LoadProjectsAndEmployees(); // Загружаем списки проектов и сотрудников
            DataContext = this; // Устанавливаем контекст данных
        }

        // Заголовок окна (зависит от режима)
        public string WindowTitle => IsEditMode ? "Редактирование задачи" : "Добавление задачи";

        // Загрузка списков проектов и сотрудников
        private void LoadProjectsAndEmployees()
        {
            using (var context = new AppDbContext())
            {
                Projects = context.Projects.ToList();
                Employees = context.Employees.ToList();
            }

            // Устанавливаем источники данных для ComboBox
            ProjectComboBox.ItemsSource = Projects;
            EmployeeComboBox.ItemsSource = Employees;
            StatusComboBox.ItemsSource = new string[] {
            "Новая",
            "В процессе" ,
                "Завершена"
            };
            // Если это режим редактирования, выбираем текущие значения
            if (IsEditMode)
            {
                ProjectComboBox.SelectedValue = Task.ProjectId;
                EmployeeComboBox.SelectedValue = Task.EmployeeId;
            }
        }

        // Сохранение задачи
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Task.Description))
            {
                MessageBox.Show("Введите описание задачи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Task.StartDate == null || Task.EndDate == null)
            {
                MessageBox.Show("Укажите даты начала и окончания задачи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Task.StartDate > Task.EndDate)
            {
                MessageBox.Show("Дата начала не может быть позже даты окончания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем задачу в базу данных
            using (var context = new AppDbContext())
            {
                if (IsEditMode)
                {
                    // Редактируем существующую задачу
                    var existingTask = context.Tasks.Find(Task.Id);
                    if (existingTask != null)
                    {
                        existingTask.Description = Task.Description;
                        existingTask.Status = Task.Status;
                        existingTask.StartDate = DateTime.SpecifyKind(Task.StartDate, DateTimeKind.Utc);
                        existingTask.EndDate = DateTime.SpecifyKind(Task.EndDate, DateTimeKind.Utc);
                        existingTask.ProjectId = Task.ProjectId;
                        existingTask.EmployeeId = Task.EmployeeId;
                    }
                }
                else
                {
                    Task.StartDate = DateTime.SpecifyKind(Task.StartDate, DateTimeKind.Utc);
                    Task.EndDate = DateTime.SpecifyKind(Task.EndDate, DateTimeKind.Utc);
                    // Добавляем новую задачу
                    context.Tasks.Add(Task);
                }

                context.SaveChanges();
            }

            DialogResult = true; // Закрываем окно с результатом "Успешно"
            Close();
        }

        // Отмена
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Закрываем окно с результатом "Отмена"
            Close();
        }
    }
}
