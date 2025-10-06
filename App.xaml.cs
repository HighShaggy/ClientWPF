using ClientWpf.Data;
using ClientWpf.Services;
using System.Windows;

namespace ClientWpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var db = new AppDbContext(); // берёт строку из App.config
            var clientService = new ClientService(db);
            var requestService = new RequestService();
            var allRequestsVM = new AllRequestsVM(requestService);

            var mainViewModel = new MainViewModel(clientService, requestService, allRequestsVM);

            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            MainWindow = mainWindow;
            mainWindow.Show();
        }
    }
}
