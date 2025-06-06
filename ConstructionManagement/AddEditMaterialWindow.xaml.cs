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
    public partial class AddEditMaterialWindow : Window
    {
        public Material Material { get; private set; } // Материал для добавления или редактирования
        public bool IsEditMode { get; private set; } // Режим редактирования (true) или добавления (false)
        public List<Supplier> Suppliers { get; set; } // Список поставщиков

        public AddEditMaterialWindow()
        {
            InitializeComponent();
            Material = new Material(); // Создаем новый материал
            IsEditMode = false; // Режим добавления
            LoadSuppliers(); // Загружаем список поставщиков
            DataContext = this; // Устанавливаем контекст данных
        }

        public AddEditMaterialWindow(Material materialToEdit)
        {
            InitializeComponent();
            Material = materialToEdit; // Редактируем существующий материал
            IsEditMode = true; // Режим редактирования
            LoadSuppliers(); // Загружаем список поставщиков
            DataContext = this; // Устанавливаем контекст данных
        }

        // Заголовок окна (зависит от режима)
        public string WindowTitle => IsEditMode ? "Редактирование материала" : "Добавление материала";

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
                SupplierComboBox.SelectedValue = Material.SupplierId;
            }
        }

        // Сохранение материала
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Material.Name))
            {
                MessageBox.Show("Введите название материала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Material.Quantity <= 0)
            {
                MessageBox.Show("Укажите корректное количество.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Material.Price <= 0)
            {
                MessageBox.Show("Укажите корректную цену.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохраняем материал в базу данных
            using (var context = new AppDbContext())
            {
                if (IsEditMode)
                {
                    // Редактируем существующий материал
                    var existingMaterial = context.Materials.Find(Material.Id);
                    if (existingMaterial != null)
                    {
                        existingMaterial.Name = Material.Name;
                        existingMaterial.Quantity = Material.Quantity;
                        existingMaterial.Price = Material.Price;
                        existingMaterial.SupplierId = Material.SupplierId;
                    }
                }
                else
                {
                    // Добавляем новый материал
                    context.Materials.Add(Material);
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