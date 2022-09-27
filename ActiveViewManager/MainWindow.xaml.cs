using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;
using System.Windows.Controls;

namespace Ribbon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UIDocument uiDoc { get; }
        public Autodesk.Revit.DB.Document doc { get; }

        public MainWindow(UIDocument UiDoc)
        {
            uiDoc = UiDoc;
            doc = UiDoc.Document;
            InitializeComponent();
            Title = "中国联合";
        }

        private void SetViewName(object sender, RoutedEventArgs e)
        {
            using(Transaction t = new Transaction(doc, "set view name"))
            {
                t.Start();
                doc.ActiveView.Name = text.Text;
                t.Commit();
            }
        }

        private void PrintView(object sender, RoutedEventArgs e)
        {
            using (Transaction t = new Transaction(doc, "print view"))
            {
                t.Start();
                doc.ActiveView.Print();
                t.Commit();
            }
        }
    }
}
