using ClientWpf.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

public class ClientVM : INotifyPropertyChanged
{
    public Client Model { get; }

    public ClientVM(Client client)
    {
        Model = client ?? throw new ArgumentNullException(nameof(client));

        if (!(Model.Requests is ObservableCollection<Request> requests))
        {
            requests = new ObservableCollection<Request>(Model.Requests);
            Model.Requests = requests;
        }
        requests.CollectionChanged += Requests_CollectionChanged;
    }

    public string Name => Model.Name;
    public string Inn => Model.Inn;
    public string BusinessAreaName => Model.BusinessArea?.Name;
    public string Note => Model.Note;
    public int RequestsCount => Model.Requests?.Count ?? 0;

    public DateTime? LastRequestDate => Model.Requests?.Any() == true
        ? Model.Requests.Max(r => r.RequestDate)
        : (DateTime?)null;

    public event PropertyChangedEventHandler PropertyChanged;

    private void Requests_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(RequestsCount));
        OnPropertyChanged(nameof(LastRequestDate));
    }

    public void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
