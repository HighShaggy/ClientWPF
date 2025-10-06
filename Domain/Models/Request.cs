using System;

namespace ClientWpf.Models
{
    public class Request
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public DateTime RequestDate { get; set; }

        public string WorkName { get; set; }

        public string WorkDescription { get; set; }

        public int StatusId { get; set; }

        public virtual Client Client { get; set; }

        public virtual RequestStatus Status { get; set; }
    }
}
