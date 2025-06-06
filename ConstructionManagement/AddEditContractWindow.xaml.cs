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
using ConstructionManagement.Data;
using ConstructionManagement.Models;

namespace ConstructionManagement
{
    public partial class AddEditContractWindow : Window
    {
        public Contract Contract { get; private set; } // Контракт для добавления или редактирования
        public bool IsEditMode { get; private set; } // Режим редактирования (true) или добавления (false)
        public List<Customer> Customers { get; set; } // Список заказчиков

        public AddEditContractWindow()
        {
            InitializeComponent();
            Contract = new Contract(); // Создаем новый контракт
            IsEditMode = false; // Режим добавления
            LoadCustomers(); // Загружаем список заказчиков
            DataContext = this; // Устанавливаем контекст данных
        }

        public AddEditContractWindow(Contract contractToEdit)
        {
            InitializeComponent();
            Contract = contractToEdit; // Редактируем существующий контракт
            IsEditMode = true; // Режим редактирования
            LoadCustomers(); // Загружаем список заказчиков
            DataContext = this; // Устанавливаем контекст данных
        }

        // Заголовок окна (зависит от режима)
        public string WindowTitle => IsEditMode ? "Редактирование контракта" : "Добавление контракта";

        // Загрузка списка заказчиков
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
                CustomerComboBox.SelectedValue = Contract.CustomersId;
            }
        }

        // Сохранение контракта
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Contract.Name))
            {
                MessageBox.Show("Введите название контракта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Contract.DateOfConclusion == null)
            {
                MessageBox.Show("Укажите дату заключения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Contract.Amount <= 0)
            {
                MessageBox.Show("Укажите корректную сумму.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем контракт в базу данных
            using (var context = new AppDbContext())
            {
                if (IsEditMode)
                {
                    // Редактируем существующий контракт
                    var existingContract = context.Contracts.Find(Contract.Id);
                    if (existingContract != null)
                    {
                        existingContract.Name = Contract.Name;
                        existingContract.DateOfConclusion = DateTime.SpecifyKind(Contract.DateOfConclusion, DateTimeKind.Utc);
                        existingContract.Amount = Contract.Amount;
                        existingContract.CustomersId = Contract.CustomersId;
                    }
                }
                else
                {
                        Contract.DateOfConclusion = DateTime.SpecifyKind(Contract.DateOfConclusion, DateTimeKind.Utc);

                    // Добавляем новый контракт
                    context.Contracts.Add(Contract);
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
