using System.Windows;

namespace ClientWpf.Views
{
    public partial class EditClientWindow : Window
    {
        public EditClientWindow()
        {
            InitializeComponent();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as EditClientViewModel;
            if (string.IsNullOrWhiteSpace(vm?.Name) || string.IsNullOrWhiteSpace(vm.Inn) || vm.SelectedBusinessArea == null)
            {
                MessageBox.Show("Заполните все обязательные поля и выберите сферу деятельности", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }
    }
}
