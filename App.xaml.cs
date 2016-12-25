using System.Windows;
using FindExtension.ViewModels;
using FindExtension.Views;

namespace FindExtension
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var window = new MainWindow {};
            var viewModel = new MainWindowViewModel();
            window.DataContext = viewModel;
            MainWindowViewModel.MainWindow = window;
            window.Show();
        }
    }
}
