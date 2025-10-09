using ClientWpf.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientWpf.Services
{
    public interface IRequestService
    {
        event Action RequestsChanged;

        Task<List<Request>> GetAllAsync();

        Task<List<Request>> GetByClientIdAsync(int clientId);

        Task<List<RequestStatus>> GetStatusesAsync();

        Task AddAsync(Request request);

        Task UpdateAsync(Request request);

        Task DeleteAsync(Request request);
    }
}
