using ClientWpf.Commands;
using ClientWpf.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

public class EditRequestViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// ViewModel для всплывающего окна добавления или редактирования заявки.
    /// </summary>
    public DateTime RequestDate { get; set; } = DateTime.Now;
    public string WorkName { get; set; }
    public string WorkDescription { get; set; }

    public ObservableCollection<RequestStatus> Statuses { get; }

    private RequestStatus _selectedStatus;
    public RequestStatus SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            if (_selectedStatus != value)
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public EditRequestViewModel(ObservableCollection<RequestStatus> statuses)
    {
        Statuses = statuses ?? new ObservableCollection<RequestStatus>();
        if (Statuses.Count > 0)
            SelectedStatus = Statuses[0];

        SaveCommand = new RelayCommand(async () => await SaveAsync());
        CancelCommand = new RelayCommand(async () => await CancelAsync());
    }

    private Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(WorkName))
        {
            MessageBox.Show("Введите наименование работы!");
            return Task.CompletedTask;
        }

        CloseWindow(true);
        return Task.CompletedTask;
    }

    private Task CancelAsync()
    {
        CloseWindow(false);
        return Task.CompletedTask;
    }

    private void CloseWindow(bool result)
    {
        foreach (Window window in Application.Current.Windows)
        {
            if (window.DataContext == this)
            {
                window.DialogResult = result;
                window.Close();
                break;
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
