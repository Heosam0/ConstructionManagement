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
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore
    ;
namespace ConstructionManagement
{
    public partial class EquipmentWindow : Window
    {
        public EquipmentWindow()
        {
            InitializeComponent();
            LoadEquipment();
        }

        private void LoadEquipment()
        {
            using (var context = new AppDbContext())
            {
                // Загружаем оборудование с поставщиками
                var equipment = context.Equipment
                    .Include(e => e.Supplier)
                    .ToList();

                EquipmentDataGrid.ItemsSource = equipment;
            }
        }

        private void AddEquipment_Click(object sender, RoutedEventArgs e)
        {
            var addEquipmentWindow = new AddEditEquipmentWindow();
            if (addEquipmentWindow.ShowDialog() == true)
            {
                LoadEquipment(); // Обновляем список оборудования
            }
        }

        private void EditEquipment_Click(object sender, RoutedEventArgs e)
        {
            var selectedEquipment = EquipmentDataGrid.SelectedItem as Equipment;
            if (selectedEquipment != null)
            {
                var editEquipmentWindow = new AddEditEquipmentWindow(selectedEquipment);
                if (editEquipmentWindow.ShowDialog() == true)
                {
                    LoadEquipment(); // Обновляем список оборудования
                }
            }
            else
            {
                MessageBox.Show("Выберите оборудование для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteEquipment_Click(object sender, RoutedEventArgs e)
        {
            var selectedEquipment = EquipmentDataGrid.SelectedItem as Equipment;
            if (selectedEquipment != null)
            {
                using (var context = new AppDbContext())
                {
                    var equipmentToDelete = context.Equipment.Find(selectedEquipment.Id);
                    if (equipmentToDelete != null)
                    {
                        context.Equipment.Remove(equipmentToDelete);
                        context.SaveChanges();
                        LoadEquipment(); // Обновляем список оборудования
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите оборудование для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}