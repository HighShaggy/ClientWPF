using System.Windows;

namespace ClientWpf.Views
{
    public partial class EditRequestWindow : Window
    {
        public EditRequestWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}


