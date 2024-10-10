using System.Windows;

namespace DesktopAssignment.Views
{
    /// <summary>
    /// Interaction logic for WarningDialog.xaml
    /// </summary>
    public partial class WarningDialog : Window
    {
        public WarningDialog(string warningMessage)
        {
            InitializeComponent();
            WarningTextBlock.Text = warningMessage;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
