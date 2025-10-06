using System.Collections.Generic;

namespace ClientWpf.Models
{
    public class BusinessArea
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
