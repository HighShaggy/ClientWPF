using ClientWpf.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class RequestViewModel : INotifyPropertyChanged
{
    public Request Model { get; }

    public DateTime RequestDate => Model.RequestDate;
    public string WorkName => Model.WorkName;
    public string WorkDescription => Model.WorkDescription;

    private RequestStatus _status;
    public RequestStatus Status
    {
        get => _status;
        set
        {
            if (_status == value) return;
            _status = value;
            Model.Status = value;
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(StatusName));
        }
    }

    public string StatusName => Status?.Name;

    private ObservableCollection<RequestStatus> _statuses;
    public ObservableCollection<RequestStatus> Statuses
    {
        get => _statuses;
        set
        {
            if (_statuses == value) return;
            _statuses = value;
            OnPropertyChanged(nameof(Statuses));
        }
    }

    public ICommand EditCommand { get; set; }
    public ICommand DeleteCommand { get; set; }

    public RequestViewModel(Request model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
        _status = model.Status;
        Statuses = new ObservableCollection<RequestStatus>();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
