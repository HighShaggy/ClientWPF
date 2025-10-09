using ClientWpf.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientWpf.Services
{
    public interface IClientService
    {
        event Action ClientsChanged;

        Task<List<Client>> GetAllAsync();

        Task AddAsync(Client client);

        Task UpdateAsync(Client client);

        Task DeleteAsync(Client client);

        Task<List<BusinessArea>> GetBusinessAreasAsync();
    }
}
