using ClientWpf.Models;
using ClientWpf.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

public class AllRequestsVM : INotifyPropertyChanged
{
    /// <summary>
    /// ViewModel для второй вкладки — «Все заявки».
    /// Управляет списком всех заявок, их статусами и фильтрацией по клиенту.
    /// </summary>
    private readonly IRequestService _requestService;
    private readonly IClientService _clientService;

    private readonly ObservableCollection<RequestVM> _allRequests = new ObservableCollection<RequestVM>();

    public ObservableCollection<RequestVM> Requests { get; } = new ObservableCollection<RequestVM>();
    public ObservableCollection<Client> Clients { get; } = new ObservableCollection<Client>();

    public ICollectionView RequestsView { get; }

    private Client _selectedClient;
    public Client SelectedClient
    {
        get => _selectedClient;
        set
        {
            if (_selectedClient == value) return;
            _selectedClient = value;
            OnPropertyChanged(nameof(SelectedClient));
            RefreshFilteredRequests();
        }
    }

    public AllRequestsVM(IRequestService requestService, IClientService clientService)
    {
        _requestService = requestService;
        _clientService = clientService;

        // Подписываемся на событие изменения заявок
        _requestService.RequestsChanged += async () => await LoadAllAsync();
        _clientService.ClientsChanged += OnClientsChanged;

        RequestsView = CollectionViewSource.GetDefaultView(Requests);
        RequestsView.SortDescriptions.Add(
            new SortDescription(nameof(RequestVM.RequestDate), ListSortDirection.Descending));

        _ = LoadAllAsync();
    }

    public async Task LoadAllAsync()
    {
        var allRequests = await _requestService.GetAllAsync();
        var statuses = await _requestService.GetStatusesAsync();

        Application.Current.Dispatcher.Invoke(() =>
        {
            _allRequests.Clear();
            Requests.Clear();
            Clients.Clear();

            foreach (var r in allRequests.OrderByDescending(x => x.RequestDate))
            {
                if (r.Status == null && r.StatusId != 0)
                    r.Status = statuses.FirstOrDefault(s => s.Id == r.StatusId);

                var vm = new RequestVM(r)
                {
                    Statuses = new ObservableCollection<RequestStatus>(statuses)
                };

                vm.PropertyChanged += async (s, e) =>
                {
                    if (e.PropertyName == nameof(RequestVM.Status))
                    {
                        vm.Model.StatusId = vm.Status?.Id ?? 0;
                        await _requestService.UpdateAsync(vm.Model);
                    }
                };

                _allRequests.Add(vm);
                Requests.Add(vm);
            }

            foreach (var c in allRequests
                .Select(r => r.Client)
                .Where(cl => cl != null)
                .GroupBy(cl => cl.Id)
                .Select(g => g.First()))
            {
                Clients.Add(c);
            }
        });
    }

    public void AddRequestToCollections(Request request, ObservableCollection<RequestStatus> statuses)
    {
        if (request.Status == null && request.StatusId != 0)
            request.Status = statuses.FirstOrDefault(s => s.Id == request.StatusId);

        var vm = new RequestVM(request)
        {
            Statuses = new ObservableCollection<RequestStatus>(statuses)
        };

        vm.PropertyChanged += async (s, e) =>
        {
            if (e.PropertyName == nameof(RequestVM.Status))
            {
                vm.Model.StatusId = vm.Status?.Id ?? 0;
                await _requestService.UpdateAsync(vm.Model);
            }
        };

        _allRequests.Insert(0, vm);
        Requests.Insert(0, vm);

        if (!Clients.Any(c => c.Id == request.ClientId) && request.Client != null)
            Clients.Add(request.Client);
    }

    private void RefreshFilteredRequests()
    {
        Requests.Clear();

        var filtered = _selectedClient == null
            ? _allRequests
            : new ObservableCollection<RequestVM>(
                _allRequests.Where(r => r.Model.ClientId == _selectedClient.Id));

        foreach (var r in filtered.OrderByDescending(r => r.RequestDate))
            Requests.Add(r);
    }
    private async void OnClientsChanged()
    {
        var allClients = await _clientService.GetAllAsync();
        Application.Current.Dispatcher.Invoke(() =>
        {
            Clients.Clear();
            foreach (var c in allClients)
                Clients.Add(c);
        });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
