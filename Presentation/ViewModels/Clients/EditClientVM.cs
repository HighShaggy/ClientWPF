using ClientWpf.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

public class EditClientVM : INotifyPropertyChanged
{
    /// <summary>
    /// ViewModel для отдельного окна CRUD клиента
    /// </summary>
    public Client Model { get; }

    public string Name
    {
        get => Model.Name;
        set
        {
            if (Model.Name != value)
            {
                Model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public string Inn
    {
        get => Model.Inn;
        set
        {
            if (Model.Inn != value)
            {
                Model.Inn = value;
                OnPropertyChanged(nameof(Inn));
            }
        }
    }

    public string Note
    {
        get => Model.Note;
        set
        {
            if (Model.Note != value)
            {
                Model.Note = value;
                OnPropertyChanged(nameof(Note));
            }
        }
    }

    public ObservableCollection<BusinessArea> BusinessAreas { get; }

    private BusinessArea _selectedBusinessArea;
    public BusinessArea SelectedBusinessArea
    {
        get => _selectedBusinessArea;
        set
        {
            if (_selectedBusinessArea != value)
            {
                _selectedBusinessArea = value;
                if (value != null)
                    Model.BusinessAreaId = value.Id;
                OnPropertyChanged(nameof(SelectedBusinessArea));
            }
        }
    }

    public EditClientVM(Client client, IEnumerable<BusinessArea> businessAreas)
    {
        Model = client ?? throw new System.ArgumentNullException(nameof(client));
        BusinessAreas = new ObservableCollection<BusinessArea>(businessAreas ?? new List<BusinessArea>());
        SelectedBusinessArea = client.BusinessArea;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
