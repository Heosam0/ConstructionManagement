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
using ConstructionManagement.Data;
using ConstructionManagement.Models;

namespace ConstructionManagement
{
    public partial class AddEditCustomerWindow : Window
    {
        public Customer Customer { get; private set; } // Заказчик для добавления или редактирования
        public bool IsEditMode { get; private set; } // Режим редактирования (true) или добавления (false)

        public AddEditCustomerWindow()
        {
            InitializeComponent();
            Customer = new Customer(); // Создаем нового заказчика
            IsEditMode = false; // Режим добавления
            DataContext = this; // Устанавливаем контекст данных
        }

        public AddEditCustomerWindow(Customer customerToEdit)
        {
            InitializeComponent();
            Customer = customerToEdit; // Редактируем существующего заказчика
            IsEditMode = true; // Режим редактирования
            DataContext = this; // Устанавливаем контекст данных
        }

        // Заголовок окна (зависит от режима)
        public string WindowTitle => IsEditMode ? "Редактирование заказчика" : "Добавление заказчика";

        // Сохранение заказчика
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Customer.LastName) || string.IsNullOrWhiteSpace(Customer.FirstName))
            {
                MessageBox.Show("Введите фамилию и имя заказчика.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем заказчика в базу данных
            using (var context = new AppDbContext())
            {
                if (IsEditMode)
                {
                    // Редактируем существующего заказчика
                    var existingCustomer = context.Customers.Find(Customer.Id);
                    if (existingCustomer != null)
                    {
                        existingCustomer.LastName = Customer.LastName;
                        existingCustomer.FirstName = Customer.FirstName;
                        existingCustomer.Patronymic = Customer.Patronymic;
                        existingCustomer.OrganizationName = Customer.OrganizationName;
                        existingCustomer.Email = Customer.Email;
                        existingCustomer.Phone = Customer.Phone;
                    }
                }
                else
                {
                    // Добавляем нового заказчика
                    context.Customers.Add(Customer);
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
