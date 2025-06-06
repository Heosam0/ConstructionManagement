using ConstructionManagement.Data;
using ConstructionManagement.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConstructionManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string enteredUsername = login.Text;
            string enteredPassword = password.Password;

            // Проверка введенных данных
            using (var context = new AppDbContext())
            {
                var employee = context.Employees
                    .FirstOrDefault(emp => emp.Username == enteredUsername && emp.Password == enteredPassword);

                if (employee != null)
                {
                    // Успешный вход
                    MessageBox.Show("Вход выполнен успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Открываем новое окно или выполняем другие действия
                    OpenMainMenu(employee);
                }
                else
                {
                    // Неверные данные
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    
        private void OpenMainMenu(Employee emp)
        {
            // Пример открытия нового окна
            var mainMenu = new MainMenuWindow(emp);
            mainMenu.Show();
            this.Close(); // Закрываем текущее окно входа
        }
    }
}
    