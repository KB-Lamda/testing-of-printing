using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testing_of_printing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            PersonDetails _model = new PersonDetails();
            _model.FirstName = "Scott";
            _model.LastName = "Peterson";
            _model.Age = 35;
            PrintPreviewViewModel vm = new PrintPreviewViewModel(_model);
            _printPreviewViewModel = vm;
            this.DataContext = _printPreviewViewModel;
            InitializeComponent();
        }

        PrintPreviewViewModel _printPreviewViewModel;

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDocument();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void PrintDocument()
        {

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() != true)
            {
                return;
            }
            this.UpdateLayout();
            this.InvalidateVisual();

            // Create a document
            FixedDocument document = new FixedDocument();
            document.DocumentPaginator.PageSize = new System.Windows.Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
            PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);

            // Store some properties of report border, which will be restored after printing report.
            FixedPage Resultspage = new FixedPage();

            Transform oldTransform = PrintStackPanel.LayoutTransform;
            System.Windows.Size oldSize = new System.Windows.Size(PrintStackPanel.ActualWidth, PrintStackPanel.ActualHeight);
            var oldMargin = PrintStackPanel.Margin;
            var oldWidth = PrintStackPanel.Width;


            try
            {
                double _reportMargin = 30;

                Resultspage.Width = document.DocumentPaginator.PageSize.Width;
                Resultspage.Height = document.DocumentPaginator.PageSize.Height;

                //PrintArea.Child = null;
                PrintArea.Children.Remove(PrintStackPanel);

                PrintStackPanel.LayoutTransform = new ScaleTransform(1, 1);

                System.Windows.Size sz = new System.Windows.Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                PrintStackPanel.Margin = new Thickness(_reportMargin);
                PrintStackPanel.Width = capabilities.PageImageableArea.ExtentWidth - 2 * _reportMargin;
                PrintStackPanel.Measure(sz);
                PrintStackPanel.Arrange(new Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));
                PrintStackPanel.UpdateLayout();


                Resultspage.Children.Add((UIElement)PrintStackPanel);
                Resultspage.UpdateLayout();

                // add the page to the document
                PageContent ReportPageContent = new PageContent();
                ((IAddChild)ReportPageContent).AddChild(Resultspage);

                document.Pages.Add(ReportPageContent);

                printDialog.PrintDocument(document.DocumentPaginator, "Report");
            }
            catch (Exception caughtException)
            {
                MessageBox.Show(caughtException.Message);
            }
            finally
            {

                PrintStackPanel.Measure(oldSize);
                PrintStackPanel.Width = oldWidth;
                PrintStackPanel.Margin = oldMargin;
                PrintStackPanel.LayoutTransform = oldTransform;

                PrintStackPanel.Arrange(new Rect(new System.Windows.Point(0, 0), oldSize));

                Resultspage.Children.Remove(PrintStackPanel);

                //PrintArea.Child = PrintStackPanel;
                PrintArea.Children.Add(PrintStackPanel);

            }
        }

    }
}
