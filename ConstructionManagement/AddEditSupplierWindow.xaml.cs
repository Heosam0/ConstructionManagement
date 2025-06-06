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
    public partial class AddEditSupplierWindow : Window
    {
        public Supplier Supplier { get; private set; } // Поставщик для добавления или редактирования
        public bool IsEditMode { get; private set; } // Режим редактирования (true) или добавления (false)

        public AddEditSupplierWindow()
        {
            InitializeComponent();
            Supplier = new Supplier(); // Создаем нового поставщика
            IsEditMode = false; // Режим добавления
            DataContext = this; // Устанавливаем контекст данных
        }

        public AddEditSupplierWindow(Supplier supplierToEdit)
        {
            InitializeComponent();
            Supplier = supplierToEdit; // Редактируем существующего поставщика
            IsEditMode = true; // Режим редактирования
            DataContext = this; // Устанавливаем контекст данных
        }

        // Заголовок окна (зависит от режима)
        public string WindowTitle => IsEditMode ? "Редактирование поставщика" : "Добавление поставщика";

        // Сохранение поставщика
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Supplier.OrganizationName))
            {
                MessageBox.Show("Введите название организации.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем поставщика в базу данных
            using (var context = new AppDbContext())
            {
                if (IsEditMode)
                {
                    // Редактируем существующего поставщика
                    var existingSupplier = context.Suppliers.Find(Supplier.Id);
                    if (existingSupplier != null)
                    {
                        existingSupplier.OrganizationName = Supplier.OrganizationName;
                        existingSupplier.Email = Supplier.Email;
                        existingSupplier.Phone = Supplier.Phone;
                    }
                }
                else
                {
                    // Добавляем нового поставщика
                    context.Suppliers.Add(Supplier);
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