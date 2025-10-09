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

            var db = new AppDbContext();
            IClientService clientService = new ClientService(db);
            IRequestService requestService = new RequestService();

            var allRequestsVM = new AllRequestsVM(requestService,clientService);
            var mainViewModel = new MainVM(clientService, requestService, allRequestsVM);

            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            MainWindow = mainWindow;
            mainWindow.Show();
            _ = mainViewModel.ClientsVM.LoadClientsAsync();
        }
    }
}
