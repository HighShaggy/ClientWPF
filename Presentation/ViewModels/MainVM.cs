using ClientWpf.Services;

public class MainVM
{
    public ClientsVM ClientsVM { get; }

    public RequestsVM RequestsVM { get; }

    public AllRequestsVM AllRequestsVM { get; }

    public MainVM(IClientService clientService, IRequestService requestService, AllRequestsVM allRequestsVM)
    {
        ClientsVM = new ClientsVM(clientService);
        AllRequestsVM = allRequestsVM;
        RequestsVM = new RequestsVM(requestService, ClientsVM, allRequestsVM);
    }
}
