using System.Windows;
using System.Windows.Controls;

namespace ClientWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var combo = sender as ComboBox;
            combo.Focus();
            combo.IsDropDownOpen = true;
        }

        private void DataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            if (e.Column.Header?.ToString() == "Статус")
            {
                if (e.EditingElement is FrameworkElement fe)
                {
                    var combo = fe.FindName("StatusCombo") as ComboBox;
                    if (combo != null)
                        combo.IsDropDownOpen = true;
                }
            }
        }
    }
}
