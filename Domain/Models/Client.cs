using System.Collections.ObjectModel;

namespace ClientWpf.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Inn { get; set; }

        public int BusinessAreaId { get; set; }

        public string Note { get; set; }

        public virtual BusinessArea BusinessArea { get; set; }

        public ObservableCollection<Request> Requests { get; set; }

        public Client()
        {
            Requests = new ObservableCollection<Request>();
        }
    }
}
