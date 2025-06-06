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
using global::ConstructionManagement.Data;
using global::ConstructionManagement.Models;
using Microsoft.EntityFrameworkCore;
namespace ConstructionManagement
{
    /// <summary>
    /// Логика взаимодействия для CustomersWindow.xaml
    /// </summary>
   

   
        public partial class CustomersWindow : Window
        {
            public CustomersWindow()
            {
                InitializeComponent();
                LoadCustomers();
            }

            private void LoadCustomers()
            {
                using (var context = new AppDbContext())
                {
                    // Загружаем заказчиков с их контрактами и проектами
                    var customers = context.Customers
                        .Include(c => c.Contracts)
                        .Include(c => c.Projects)
                        .ToList();

                    CustomersDataGrid.ItemsSource = customers;
                }
            }

            private void AddCustomer_Click(object sender, RoutedEventArgs e)
            {
                var addCustomerWindow = new AddEditCustomerWindow();
                if (addCustomerWindow.ShowDialog() == true)
                {
                    LoadCustomers(); // Обновляем список заказчиков
                }
            }

            private void EditCustomer_Click(object sender, RoutedEventArgs e)
            {
                var selectedCustomer = CustomersDataGrid.SelectedItem as Customer;
                if (selectedCustomer != null)
                {
                    var editCustomerWindow = new AddEditCustomerWindow(selectedCustomer);
                    if (editCustomerWindow.ShowDialog() == true)
                    {
                        LoadCustomers(); // Обновляем список заказчиков
                    }
                }
                else
                {
                    MessageBox.Show("Выберите заказчика для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
            {
                var selectedCustomer = CustomersDataGrid.SelectedItem as Customer;
                if (selectedCustomer != null)
                {
                    using (var context = new AppDbContext())
                    {
                        var customerToDelete = context.Customers
                            .Include(c => c.Contracts)
                            .Include(c => c.Projects)
                            .FirstOrDefault(c => c.Id == selectedCustomer.Id);

                        if (customerToDelete != null)
                        {
                            // Удаляем заказчика, если у него нет связанных контрактов и проектов
                            if (customerToDelete.Contracts.Count == 0 && customerToDelete.Projects.Count == 0)
                            {
                                context.Customers.Remove(customerToDelete);
                                context.SaveChanges();
                                LoadCustomers(); // Обновляем список заказчиков
                            }
                            else
                            {
                                MessageBox.Show("Невозможно удалить заказчика, так как у него есть связанные контракты или проекты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выберите заказчика для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
