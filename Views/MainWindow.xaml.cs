using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FindExtension.ViewModels;
using System.Linq;
using FindExtension.Models;

namespace FindExtension.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;

        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var extensions = dataGrid1.ItemsSource.Cast<ExtensionModel>();
            var content = string.Join("\n", extensions);

            System.IO.File.WriteAllText("ext.dat", content);
        }

    }
}
