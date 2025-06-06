using ConstructionManagement.Data;
using ConstructionManagement.Models;
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
    public partial class AddEditProjectWindow : Window
    {
        public Project Project { get; private set; } // Проект для добавления или редактирования
        public bool IsEditMode { get; private set; } // Режим редактирования (true) или добавления (false)
        public List<Customer> Customers { get; set; } // Список заказчиков

        public AddEditProjectWindow()
        {
            InitializeComponent();
            Project = new Project(); // Создаем новый проект
            IsEditMode = false; // Режим добавления
            LoadCustomers(); // Загружаем список заказчиков
            DataContext = this; // Устанавливаем контекст данных
        }

        public AddEditProjectWindow(Project projectToEdit)
        {
            InitializeComponent();
            Project = projectToEdit; // Редактируем существующий проект
            IsEditMode = true; // Режим редактирования
            LoadCustomers(); // Загружаем список заказчиков
            DataContext = this; // Устанавливаем контекст данных
        }

        // Заголовок окна (зависит от режима)
        public string WindowTitle => IsEditMode ? "Редактирование проекта" : "Добавление проекта";

        private void LoadCustomers()
        {
            using (var context = new AppDbContext())
            {
                Customers = context.Customers.ToList();
            }

            // Устанавливаем источник данных для ComboBox
            CustomerComboBox.ItemsSource = Customers;

            // Если это режим редактирования, выбираем текущего заказчика
            if (IsEditMode)
            {
                CustomerComboBox.SelectedValue = Project.CustomersId;
            }
        }
        // Сохранение проекта
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Project.Name))
            {
                MessageBox.Show("Введите название проекта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Project.StartDate == null || Project.EndDate == null)
            {
                MessageBox.Show("Укажите даты начала и окончания проекта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Project.StartDate > Project.EndDate)
            {
                MessageBox.Show("Дата начала не может быть позже даты окончания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем проект в базу данных
            using (var context = new AppDbContext())
            {
                if (IsEditMode)
                {
                    // Редактируем существующий проект
                    var existingProject = context.Projects.Find(Project.Id);
                    if (existingProject != null)
                    {
                        existingProject.Name = Project.Name;
                        existingProject.Description = Project.Description;
                        existingProject.StartDate = DateTime.SpecifyKind(existingProject.StartDate, DateTimeKind.Utc);
                        existingProject.EndDate = DateTime.SpecifyKind(existingProject.EndDate, DateTimeKind.Utc);
                        existingProject.CustomersId = Project.CustomersId;
                    }
                }
                else
                {
                    Project.StartDate = DateTime.SpecifyKind(Project.StartDate, DateTimeKind.Utc);
                    Project.EndDate = DateTime.SpecifyKind(Project.EndDate, DateTimeKind.Utc);
                    context.Projects.Add(Project);
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

