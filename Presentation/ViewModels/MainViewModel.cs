using ClientWpf.Services;

public class MainViewModel
{
    public ClientsVM ClientsVM { get; }

    public RequestsVM RequestsVM { get; }

    public AllRequestsVM AllRequestsVM { get; }

    public MainViewModel(ClientService clientService, RequestService requestService, AllRequestsVM allRequestsVM)
    {
        ClientsVM = new ClientsVM(clientService);

        RequestsVM = new RequestsVM(requestService, ClientsVM, allRequestsVM);

        AllRequestsVM = new AllRequestsVM(requestService);
    }
}
