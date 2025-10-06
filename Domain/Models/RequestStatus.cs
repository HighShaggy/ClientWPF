using System.Collections.Generic;

namespace ClientWpf.Models
{
    public class RequestStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}
