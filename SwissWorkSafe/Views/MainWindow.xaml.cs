using System.Windows;
using SwissWorkSafe.Models.Articles;
using SwissWorkSafe.ViewModels;

namespace SwissWorkSafe
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

    }
}