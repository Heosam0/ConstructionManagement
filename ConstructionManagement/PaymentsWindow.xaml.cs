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
    public partial class PaymentsWindow : Window
    {
        public PaymentsWindow()
        {
            InitializeComponent();
            LoadPayments();
        }

        private void LoadPayments()
        {
            using (var context = new AppDbContext())
            {
                // Загружаем платежи с их контрактами
                var payments = context.Payments
                    .Include(p => p.Contract)
                    .ToList();

                PaymentsDataGrid.ItemsSource = payments;
            }
        }

        private void AddPayment_Click(object sender, RoutedEventArgs e)
        {
            var addPaymentWindow = new AddEditPaymentWindow();
            if (addPaymentWindow.ShowDialog() == true)
            {
                LoadPayments(); // Обновляем список платежей
            }
        }

        private void EditPayment_Click(object sender, RoutedEventArgs e)
        {
            var selectedPayment = PaymentsDataGrid.SelectedItem as Payment;
            if (selectedPayment != null)
            {
                var editPaymentWindow = new AddEditPaymentWindow(selectedPayment);
                if (editPaymentWindow.ShowDialog() == true)
                {
                    LoadPayments(); // Обновляем список платежей
                }
            }
            else
            {
                MessageBox.Show("Выберите платеж для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeletePayment_Click(object sender, RoutedEventArgs e)
        {
            var selectedPayment = PaymentsDataGrid.SelectedItem as Payment;
            if (selectedPayment != null)
            {
                using (var context = new AppDbContext())
                {
                    var paymentToDelete = context.Payments.Find(selectedPayment.Id);
                    if (paymentToDelete != null)
                    {
                        context.Payments.Remove(paymentToDelete);
                        context.SaveChanges();
                        LoadPayments(); // Обновляем список платежей
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите платеж для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
