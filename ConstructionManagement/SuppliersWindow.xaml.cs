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
using Microsoft.EntityFrameworkCore;
using ConstructionManagement.Data;
using ConstructionManagement.Models;

namespace ConstructionManagement
{
    public partial class SuppliersWindow : Window
    {
        public SuppliersWindow()
        {
            InitializeComponent();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            using (var context = new AppDbContext())
            {
                // Загружаем поставщиков с их оборудованием и материалами
                var suppliers = context.Suppliers
                    .Include(s => s.Equipment)
                    .Include(s => s.Materials)
                    .ToList();

                SuppliersDataGrid.ItemsSource = suppliers;
            }
        }

        private void AddSupplier_Click(object sender, RoutedEventArgs e)
        {
            var addSupplierWindow = new AddEditSupplierWindow();
            if (addSupplierWindow.ShowDialog() == true)
            {
                LoadSuppliers(); // Обновляем список поставщиков
            }
        }

        private void EditSupplier_Click(object sender, RoutedEventArgs e)
        {
            var selectedSupplier = SuppliersDataGrid.SelectedItem as Supplier;
            if (selectedSupplier != null)
            {
                var editSupplierWindow = new AddEditSupplierWindow(selectedSupplier);
                if (editSupplierWindow.ShowDialog() == true)
                {
                    LoadSuppliers(); // Обновляем список поставщиков
                }
            }
            else
            {
                MessageBox.Show("Выберите поставщика для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteSupplier_Click(object sender, RoutedEventArgs e)
        {
            var selectedSupplier = SuppliersDataGrid.SelectedItem as Supplier;
            if (selectedSupplier != null)
            {
                using (var context = new AppDbContext())
                {
                    var supplierToDelete = context.Suppliers
                        .Include(s => s.Equipment)
                        .Include(s => s.Materials)
                        .FirstOrDefault(s => s.Id == selectedSupplier.Id);

                    if (supplierToDelete != null)
                    {
                        // Удаляем поставщика, если у него нет связанного оборудования и материалов
                        if (supplierToDelete.Equipment.Count == 0 && supplierToDelete.Materials.Count == 0)
                        {
                            context.Suppliers.Remove(supplierToDelete);
                            context.SaveChanges();
                            LoadSuppliers(); // Обновляем список поставщиков
                        }
                        else
                        {
                            MessageBox.Show("Невозможно удалить поставщика, так как у него есть связанное оборудование или материалы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите поставщика для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
