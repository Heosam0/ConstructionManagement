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
using ConstructionManagement.Data;
using ConstructionManagement.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConstructionManagement
{
    public partial class AddEditPaymentWindow : Window
    {
        public Payment Payment { get; private set; } // Платеж для добавления или редактирования
        public bool IsEditMode { get; private set; } // Режим редактирования (true) или добавления (false)
        public List<Contract> Contracts { get; set; } // Список контрактов

        public AddEditPaymentWindow()
        {
            InitializeComponent();
            Payment = new Payment(); // Создаем новый платеж
            IsEditMode = false; // Режим добавления
            LoadContracts(); // Загружаем список контрактов
            DataContext = this; // Устанавливаем контекст данных
        }

        public AddEditPaymentWindow(Payment paymentToEdit)
        {
            InitializeComponent();
            Payment = paymentToEdit; // Редактируем существующий платеж
            IsEditMode = true; // Режим редактирования
            LoadContracts(); // Загружаем список контрактов
            DataContext = this; // Устанавливаем контекст данных
        }

        // Заголовок окна (зависит от режима)
        public string WindowTitle => IsEditMode ? "Редактирование платежа" : "Добавление платежа";

        // Загрузка списка контрактов
        private void LoadContracts()
        {
            using (var context = new AppDbContext())
            {
                Contracts = context.Contracts.ToList();
            }

            // Устанавливаем источник данных для ComboBox
            ContractComboBox.ItemsSource = Contracts;
            TypeComboBox.ItemsSource = new string[]
            {
                "Наличными", "Безнал"
            };
            // Если это режим редактирования, выбираем текущий контракт
            if (IsEditMode)
            {
                ContractComboBox.SelectedValue = Payment.ContractId;
            }
        }

        // Сохранение платежа
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (Payment.Amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму платежа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Payment.Type) || (Payment.Type.ToString() != "Наличными" && Payment.Type.ToString() != "Безнал"))
            {
                MessageBox.Show("Выберите тип платежа (Terminal или Cash).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (Payment.Date == null)
            {
                MessageBox.Show("Укажите дату платежа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Payment.ContractId == 0)
            {
                MessageBox.Show("Выберите контракт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new AppDbContext())
            {
                if (IsEditMode)
                {
                    // Редактируем существующий платеж
                    var existingPayment = context.Payments.Find(Payment.Id);
                    if (existingPayment != null)
                    {
                        existingPayment.Amount = Payment.Amount;
                        existingPayment.Type = Payment.Type;
                        existingPayment.Date = DateTime.SpecifyKind(Payment.Date, DateTimeKind.Utc);
                        existingPayment.ContractId = Payment.ContractId;
                    }
                }
                else
                {
                    Payment.Date = DateTime.SpecifyKind(Payment.Date, DateTimeKind.Utc);
                    // Добавляем новый платеж
                    context.Payments.Add(Payment);
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