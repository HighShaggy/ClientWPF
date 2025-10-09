using ClientWpf.Commands;
using ClientWpf.Models;
using ClientWpf.Services;
using ClientWpf.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

public class RequestsVM
{
    /// <summary>
    /// ViewModel для первой вкладки — заявок выбранного клиента.
    /// Обновляет счётчики и даты последней заявки у клиента.
    /// </summary>
    private readonly IRequestService _requestService;
    private readonly ClientsVM _clientsVM;
    private readonly AllRequestsVM _allRequestsVM;

    public ObservableCollection<RequestVM> RequestsForSelectedClient { get; private set; }

    public ICommand AddRequestCommand { get; private set; }
    public ICommand DeleteRequestCommand { get; private set; }

    public RequestsVM(IRequestService requestService, ClientsVM clientsVM, AllRequestsVM allRequestsVM)
    {
        _requestService = requestService;
        _clientsVM = clientsVM;
        _allRequestsVM = allRequestsVM;

        RequestsForSelectedClient = new ObservableCollection<RequestVM>();

        AddRequestCommand = new RelayCommand(async () => await AddRequestAsync());
        DeleteRequestCommand = new RelayCommand<RequestVM>(async r => await DeleteRequestAsync(r));

        _clientsVM.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(_clientsVM.SelectedClient))
                RefreshRequestsForSelectedClient();
        };
    }

    private void RefreshRequestsForSelectedClient()
    {
        RequestsForSelectedClient.Clear();

        var selected = _clientsVM.SelectedClient;
        if (selected == null)
            return;

        foreach (var r in selected.Model.Requests.OrderByDescending(x => x.RequestDate))
        {
            RequestsForSelectedClient.Add(new RequestVM(r));
        }
    }

    private async Task AddRequestAsync()
    {
        var selected = _clientsVM.SelectedClient;
        if (selected == null)
            return;

        var statuses = new ObservableCollection<RequestStatus>(await _requestService.GetStatusesAsync());
        var vm = new EditRequestVM(statuses);
        var window = new EditRequestWindow { DataContext = vm };

        if (window.ShowDialog() != true)
            return;

        var request = new Request
        {
            ClientId = selected.Model.Id,
            RequestDate = vm.RequestDate,
            WorkName = vm.WorkName,
            WorkDescription = vm.WorkDescription,
            StatusId = vm.SelectedStatus != null ? vm.SelectedStatus.Id : 0
        };

        await _requestService.AddAsync(request);
        _allRequestsVM.AddRequestToCollections(request, statuses);

        selected.Model.Requests.Add(request);
        RefreshRequestsForSelectedClient();
        selected.OnPropertyChanged(nameof(ClientVM.RequestsCount));
    }

    private async Task DeleteRequestAsync(RequestVM requestVM)
    {
        var selected = _clientsVM.SelectedClient;
        if (requestVM == null || selected == null)
            return;

        await _requestService.DeleteAsync(requestVM.Model);

        selected.Model.Requests.Remove(requestVM.Model);
        RefreshRequestsForSelectedClient();

        selected.OnPropertyChanged(nameof(ClientVM.RequestsCount));
        selected.OnPropertyChanged(nameof(ClientVM.LastRequestDate));
    }
}
