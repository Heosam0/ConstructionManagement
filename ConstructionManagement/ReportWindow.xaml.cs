using System;
using System.IO;
using System.Linq;
using System.Windows;
using ClosedXML.Excel;
using ConstructionManagement.Data;
using ConstructionManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionManagement
{
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
            LoadReportData();
        }

        private void LoadReportData()
        {
            using (var context = new AppDbContext())
            {
                var projects = context.Projects
                    .Include(p => p.Customers) // Загружаем связанных заказчиков
                    .Include(p => p.Tasks)     // Загружаем связанные задачи
                    .Select(p => new ProjectReportViewModel
                    {
                        Name = p.Name,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        Customers = p.Customers,
                        Tasks = p.Tasks.ToList()
                    })
                    .ToList();

                ReportDataGrid.ItemsSource = projects;
            }
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем новую книгу Excel
                using (var workbook = new XLWorkbook())
                {
                    // Добавляем лист в книгу
                    var worksheet = workbook.Worksheets.Add("Отчет по проектам");

                    // Заголовки столбцов
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "Название проекта";
                    worksheet.Cell(1, 3).Value = "Заказчик";
                    worksheet.Cell(1, 4).Value = "Дата начала";
                    worksheet.Cell(1, 5).Value = "Дата окончания";
                    worksheet.Cell(1, 6).Value = "Количество задач";

                    // Данные
                    var projects = ReportDataGrid.ItemsSource as List<ProjectReportViewModel>;
                    if (projects != null)
                    {
                        for (int i = 0; i < projects.Count; i++)
                        {
                            var project = projects[i];
                            worksheet.Cell(i + 2, 2).Value = project.Name;
                            worksheet.Cell(i + 2, 3).Value = project.Customers?.OrganizationName;
                            worksheet.Cell(i + 2, 4).Value = project.StartDate.ToString("dd.MM.yyyy");
                            worksheet.Cell(i + 2, 5).Value = project.EndDate.ToString("dd.MM.yyyy");
                            worksheet.Cell(i + 2, 6).Value = project.Tasks.Count;
                        }
                    }

                    // Сохраняем файл
                    var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "Excel files (*.xlsx)|*.xlsx",
                        FileName = "Отчет_по_проектам.xlsx"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        workbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Отчет успешно экспортирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Модель для отображения данных в отчете
    public class ProjectReportViewModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Customer Customers { get; set; }
        public List<Models.Task> Tasks { get; set; }
    }
}