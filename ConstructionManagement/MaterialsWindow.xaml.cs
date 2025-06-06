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
    public partial class MaterialsWindow : Window
    {
        public MaterialsWindow()
        {
            InitializeComponent();
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            using (var context = new AppDbContext())
            {
                // Загружаем материалы с поставщиками
                var materials = context.Materials
                    .Include(m => m.Supplier)
                    .ToList();

                MaterialsDataGrid.ItemsSource = materials;
            }
        }

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            var addMaterialWindow = new AddEditMaterialWindow();
            if (addMaterialWindow.ShowDialog() == true)
            {
                LoadMaterials(); // Обновляем список материалов
            }
        }

        private void EditMaterial_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaterial = MaterialsDataGrid.SelectedItem as Material;
            if (selectedMaterial != null)
            {
                var editMaterialWindow = new AddEditMaterialWindow(selectedMaterial);
                if (editMaterialWindow.ShowDialog() == true)
                {
                    LoadMaterials(); // Обновляем список материалов
                }
            }
            else
            {
                MessageBox.Show("Выберите материал для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            var selectedMaterial = MaterialsDataGrid.SelectedItem as Material;
            if (selectedMaterial != null)
            {
                using (var context = new AppDbContext())
                {
                    var materialToDelete = context.Materials.Find(selectedMaterial.Id);
                    if (materialToDelete != null)
                    {
                        context.Materials.Remove(materialToDelete);
                        context.SaveChanges();
                        LoadMaterials(); // Обновляем список материалов
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите материал для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
