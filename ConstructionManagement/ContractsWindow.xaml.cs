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
    public partial class ContractsWindow : Window
    {
        public ContractsWindow()
        {
            InitializeComponent();
            LoadContracts();
        }

        private void LoadContracts()
        {
            using (var context = new AppDbContext())
            {
                // Загружаем контракты с заказчиками и платежами
                var contracts = context.Contracts
                    .Include(c => c.Customers)
                    .Include(c => c.Payments)
                    .ToList();

                ContractsDataGrid.ItemsSource = contracts;
            }
        }

        private void AddContract_Click(object sender, RoutedEventArgs e)
        {
            var addContractWindow = new AddEditContractWindow();
            if (addContractWindow.ShowDialog() == true)
            {
                LoadContracts(); // Обновляем список контрактов
            }
        }

        private void EditContract_Click(object sender, RoutedEventArgs e)
        {
            var selectedContract = ContractsDataGrid.SelectedItem as Contract;
            if (selectedContract != null)
            {
                var editContractWindow = new AddEditContractWindow(selectedContract);
                if (editContractWindow.ShowDialog() == true)
                {
                    LoadContracts(); // Обновляем список контрактов
                }
            }
            else
            {
                MessageBox.Show("Выберите контракт для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteContract_Click(object sender, RoutedEventArgs e)
        {
            var selectedContract = ContractsDataGrid.SelectedItem as Contract;
            if (selectedContract != null)
            {
                using (var context = new AppDbContext())
                {
                    var contractToDelete = context.Contracts
                        .Include(c => c.Payments)
                        .FirstOrDefault(c => c.Id == selectedContract.Id);

                    if (contractToDelete != null)
                    {
                        // Удаляем контракт, если у него нет связанных платежей
                        if (contractToDelete.Payments.Count == 0)
                        {
                            context.Contracts.Remove(contractToDelete);
                            context.SaveChanges();
                            LoadContracts(); // Обновляем список контрактов
                        }
                        else
                        {
                            MessageBox.Show("Невозможно удалить контракт, так как у него есть связанные платежи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите контракт для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
