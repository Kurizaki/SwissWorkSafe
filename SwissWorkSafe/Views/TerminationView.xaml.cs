using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace SwissWorkSafe.Views
{
    public partial class TerminationView : UserControl
    {
        public TerminationView()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

    }
}