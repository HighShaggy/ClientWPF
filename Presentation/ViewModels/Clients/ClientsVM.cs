using ClientWpf.Commands;
using ClientWpf.Models;
using ClientWpf.Services;
using ClientWpf.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

public class ClientsVM : INotifyPropertyChanged
{
    /// <summary>
    /// ViewModel для основного окна работы с клиентами.
    /// Отвечает за загрузку, отображение и управление списком клиентов
    /// </summary>
    private readonly ClientService _clientService;

    private ClientViewModel _selectedClient;

    public ICollectionView ClientsView { get; }
    public string ClientNote => SelectedClient?.Note;

    public ObservableCollection<ClientViewModel> Clients { get; } = new ObservableCollection<ClientViewModel>();

    public ClientsVM(ClientService clientService)
    {
        _clientService = clientService;
        _ = LoadClientsAsync();
        ClientsView = CollectionViewSource.GetDefaultView(Clients);
        ClientsView.SortDescriptions.Add(new SortDescription(nameof(ClientViewModel.Name), ListSortDirection.Ascending));
    }

    public ClientViewModel SelectedClient
    {
        get => _selectedClient;
        set
        {
            if (_selectedClient == value) return;
            _selectedClient = value;
            OnPropertyChanged(nameof(SelectedClient));
            OnPropertyChanged(nameof(ClientNote));
        }
    }

    public async Task LoadClientsAsync()
    {
        var clients = await _clientService.GetAllAsync();
        Clients.Clear();
        foreach (var c in clients)
        {
            c.Requests = new ObservableCollection<Request>(c.Requests);
            Clients.Add(new ClientViewModel(c));
        }
    }

    public ICommand AddClientCommand => new RelayCommand(async () => await AddClientAsync());
    public ICommand EditClientCommand => new RelayCommand<ClientViewModel>(async c => await EditClientAsync(c), c => c != null);
    public ICommand DeleteClientCommand => new RelayCommand<ClientViewModel>(async c => await DeleteClientAsync(c), c => c != null);

    private async Task AddClientAsync()
    {
        var client = new Client();
        var vm = new EditClientViewModel(client, await _clientService.GetBusinessAreasAsync());
        var window = new EditClientWindow { DataContext = vm };
        if (window.ShowDialog() == true)
        {
            var selectedArea = vm.BusinessAreas.FirstOrDefault(b => b.Id == client.BusinessAreaId);
            client.BusinessArea = selectedArea;
            await _clientService.AddAsync(client);
            var clientVM = new ClientViewModel(client);
            Clients.Add(clientVM);
        }
    }

    private async Task EditClientAsync(ClientViewModel client)
    {
        if (client == null) return;
        var vm = new EditClientViewModel(client.Model, await _clientService.GetBusinessAreasAsync());
        var window = new EditClientWindow { DataContext = vm };
        if (window.ShowDialog() == true)
        {
            await _clientService.UpdateAsync(vm.Model);
            OnPropertyChanged(nameof(Clients));
        }
    }

    private async Task DeleteClientAsync(ClientViewModel client)
    {
        if (client == null) return;
        await _clientService.DeleteAsync(client.Model);
        Clients.Remove(client);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
