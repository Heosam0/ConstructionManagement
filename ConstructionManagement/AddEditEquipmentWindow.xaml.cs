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

namespace ConstructionManagement
{
    public partial class AddEditEquipmentWindow : Window
    {
        public Equipment Equipment { get; private set; } // Оборудование для добавления или редактирования
        public bool IsEditMode { get; private set; } // Режим редактирования (true) или добавления (false)
        public List<Supplier> Suppliers { get; set; } // Список поставщиков

        public AddEditEquipmentWindow()
        {
            InitializeComponent();
            Equipment = new Equipment(); // Создаем новое оборудование
            IsEditMode = false; // Режим добавления
            LoadSuppliers(); // Загружаем список поставщиков
            DataContext = this; // Устанавливаем контекст данных
        }

        public AddEditEquipmentWindow(Equipment equipmentToEdit)
        {
            InitializeComponent();
            Equipment = equipmentToEdit; // Редактируем существующее оборудование
            IsEditMode = true; // Режим редактирования
            LoadSuppliers(); // Загружаем список поставщиков
            DataContext = this; // Устанавливаем контекст данных
        }

        // Заголовок окна (зависит от режима)
        public string WindowTitle => IsEditMode ? "Редактирование оборудования" : "Добавление оборудования";

        // Загрузка списка поставщиков
        private void LoadSuppliers()
        {
            using (var context = new AppDbContext())
            {
                Suppliers = context.Suppliers.ToList();
            }

            // Устанавливаем источник данных для ComboBox
            SupplierComboBox.ItemsSource = Suppliers;

            // Если это режим редактирования, выбираем текущего поставщика
            if (IsEditMode)
            {
                SupplierComboBox.SelectedValue = Equipment.SupplierId;
            }
        }

        // Сохранение оборудования
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Equipment.Name))
            {
                MessageBox.Show("Введите название оборудования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Equipment.DateOfPurchase == null)
            {
                MessageBox.Show("Укажите дату покупки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Equipment.Cost <= 0)
            {
                MessageBox.Show("Укажите корректную стоимость.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем оборудование в базу данных
            using (var context = new AppDbContext())
            {
                if (IsEditMode)
                {
                    // Редактируем существующее оборудование
                    var existingEquipment = context.Equipment.Find(Equipment.Id);
                    if (existingEquipment != null)
                    {
                        existingEquipment.Name = Equipment.Name;
                        existingEquipment.DateOfPurchase = DateTime.SpecifyKind(Equipment.DateOfPurchase, DateTimeKind.Utc);
                        existingEquipment.Cost = Equipment.Cost;
                        existingEquipment.SupplierId = Equipment.SupplierId;
                    }
                }
                else
                {
                    Equipment.DateOfPurchase = DateTime.SpecifyKind(Equipment.DateOfPurchase, DateTimeKind.Utc);
                    // Добавляем новое оборудование
                    context.Equipment.Add(Equipment);
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