using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP01_223_Nikolaeva.Model;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace UP01_223_Nikolaeva.Pages
{
    /// <summary>
    /// Логика взаимодействия для DiagrammPage.xaml
    /// </summary>
    public partial class DiagrammPage : Page
    {
        public DiagrammPage()
        {
            InitializeComponent();
            ChartOrders.ChartAreas.Add(new ChartArea("MainArea"));

            var series = new Series("Заказы")
            {
                IsValueShownAsLabel = true, 
                Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold)
            };
            ChartOrders.Series.Add(series);

            CmbDiagram.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
            CmbDiagram.SelectedIndex = 10;
        }
        private void CmbDiagram_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbDiagram.SelectedItem is SeriesChartType selectedType)
            {
                var series = ChartOrders.Series.FirstOrDefault();
                if (series == null) return;

                series.ChartType = selectedType;
                series.Points.Clear();

                var statuses = Core.context.OrderStatus.ToList();
                foreach (var status in statuses)
                {
                    int ordersCount = Core.context.Order.Count(o => o.StatusID == status.ID);

                    series.Points.AddXY(status.Name, ordersCount);
                }
            }
        }
        private void CreateWordReport(Word.Document doc)
        {
            var allStatuses = Core.context.OrderStatus.ToList();

            Word.Paragraph mainTitle = doc.Paragraphs.Add();
            mainTitle.Range.Text = "Отчет по заказам цветочного магазина";
            mainTitle.set_Style("Заголовок 1");
            mainTitle.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            mainTitle.Range.InsertParagraphAfter();

            foreach (var status in allStatuses)
            {
                Word.Paragraph statusTitle = doc.Paragraphs.Add();
                statusTitle.Range.Text = $"Статус: {status.Name}";
                statusTitle.set_Style("Заголовок 2");
                statusTitle.Range.InsertParagraphAfter();

                var statusOrders = Core.context.Order.Where(o => o.StatusID == status.ID).ToList();
                if (statusOrders.Count == 0)
                {
                    Word.Paragraph noOrders = doc.Paragraphs.Add();
                    noOrders.Range.Text = "Заказы в данном статусе отсутствуют.\n";
                    noOrders.Range.InsertParagraphAfter();
                    continue;
                }

                Word.Table ordersTable = doc.Tables.Add(doc.Paragraphs.Add().Range, statusOrders.Count + 1, 4);
                ordersTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                ordersTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

                ordersTable.Cell(1, 1).Range.Text = "Номер заказа (ID)";
                ordersTable.Cell(1, 2).Range.Text = "Дата создания";
                ordersTable.Cell(1, 3).Range.Text = "Дата доставки";
                ordersTable.Cell(1, 4).Range.Text = "Итоговая стоимость";
                ordersTable.Rows[1].Range.Font.Bold = 1;

                for (int i = 0; i < statusOrders.Count; i++)
                {
                    ordersTable.Cell(i + 2, 1).Range.Text = statusOrders[i].ID.ToString();
                    ordersTable.Cell(i + 2, 2).Range.Text = statusOrders[i].Date.ToString("dd.MM.yyyy HH:mm");
                    ordersTable.Cell(i + 2, 3).Range.Text = statusOrders[i].DeliveryDate.ToString("dd.MM.yyyy HH:mm");
                    ordersTable.Cell(i + 2, 4).Range.Text = statusOrders[i].TotalPrice.ToString("N2") + " руб.";
                }

                doc.Paragraphs.Add().Range.InsertParagraphAfter();
                var maxOrder = statusOrders.OrderByDescending(o => o.TotalPrice).FirstOrDefault();
                if (maxOrder != null)
                {
                    Word.Paragraph maxParagraph = doc.Paragraphs.Add();
                    maxParagraph.Range.Text = $"Самый дорогой заказ в статусе '{status.Name}': №{maxOrder.ID} на сумму {maxOrder.TotalPrice:N2} руб.";
                    maxParagraph.Range.Font.Color = Word.WdColor.wdColorDarkRed;
                    maxParagraph.Range.InsertParagraphAfter();
                }

                if (status != allStatuses.LastOrDefault())
                    doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
            }
        }


        private void BtnWord_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Документ Word (*.docx)|*.docx";
            sfd.FileName = "Отчет_Заказы";
            sfd.Title = "Сохранение отчета Word";

            if (sfd.ShowDialog() != true) return;
            string filePath = sfd.FileName;

            var wordApp = new Word.Application();
            wordApp.Visible = false;
            Word.Document doc = wordApp.Documents.Add();

            try
            {
                CreateWordReport(doc); 
                doc.SaveAs2(filePath);

                MessageBox.Show("Файл Word успешно сохранен!");
                wordApp.Visible = true; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
                object saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
                doc.Close(ref saveChanges);
                wordApp.Quit(ref saveChanges);
            }
        }
        private void BtnPdf_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Файл PDF (*.pdf)|*.pdf";
            sfd.FileName = "Отчет_Заказы";
            sfd.Title = "Сохранение отчета PDF";

            if (sfd.ShowDialog() != true) return;
            string filePath = sfd.FileName;

            var wordApp = new Word.Application();
            wordApp.Visible = false; 
            Word.Document doc = wordApp.Documents.Add();

            try
            {
                CreateWordReport(doc);
                doc.SaveAs2(filePath, Word.WdExportFormat.wdExportFormatPDF);

                MessageBox.Show("Файл PDF успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                //System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                object saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
                doc.Close(ref saveChanges);
                wordApp.Quit(ref saveChanges);
            }
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Книга Excel (*.xlsx)|*.xlsx";
            sfd.FileName = "Отчет_Заказы";
            sfd.Title = "Сохранение отчета Excel";

            if (sfd.ShowDialog() != true) return;
            string filePath = sfd.FileName;

            var allStatuses = Core.context.OrderStatus.ToList();
            var excelApp = new Excel.Application();
            excelApp.SheetsInNewWorkbook = allStatuses.Count;
            Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);

            try
            {
                for (int i = 0; i < allStatuses.Count; i++)
                {
                    var status = allStatuses[i];
                    Excel.Worksheet worksheet = excelApp.Worksheets.Item[i + 1];
                    worksheet.Name = status.Name;

                    int currentRow = 1;

                    worksheet.Cells[currentRow, 1] = "ID Заказа";
                    worksheet.Cells[currentRow, 2] = "Дата создания";
                    worksheet.Cells[currentRow, 3] = "Дата доставки";
                    worksheet.Cells[currentRow, 4] = "Сумма заказа";

                    Excel.Range headerRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, 4]];
                    headerRange.Font.Bold = true;
                    currentRow++;

                    var statusOrders = Core.context.Order.Where(o => o.StatusID == status.ID).ToList();
                    int startSumRow = currentRow;

                    foreach (var order in statusOrders)
                    {
                        worksheet.Cells[currentRow, 1] = order.ID;
                        worksheet.Cells[currentRow, 2] = order.Date.ToString("dd.MM.yyyy HH:mm");
                        worksheet.Cells[currentRow, 3] = order.DeliveryDate.ToString("dd.MM.yyyy HH:mm");
                        worksheet.Cells[currentRow, 4] = order.TotalPrice;
                        (worksheet.Cells[currentRow, 4] as Excel.Range).NumberFormat = "0.00";
                        currentRow++;
                    }
                    if (statusOrders.Count > 0)
                    {
                        worksheet.Cells[currentRow, 3] = "ИТОГО по статусу:";
                        worksheet.Cells[currentRow, 3].Font.Bold = true;
                        worksheet.Cells[currentRow, 4].Formula = $"=SUM(D{startSumRow}:D{currentRow - 1})";
                        worksheet.Cells[currentRow, 4].Font.Bold = true;
                    }
                    worksheet.Columns.AutoFit();
                }
                workbook.SaveAs(filePath);
                MessageBox.Show("Файл Excel успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                excelApp.Visible = true; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения Excel: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                workbook.Close(false);
                excelApp.Quit();
            }
        }
    }
}
                    

