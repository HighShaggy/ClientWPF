using ClientWpf.Data;
using ClientWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientWpf.Services
{
    public class ClientService
    {
        private readonly AppDbContext _db;

        public ClientService(AppDbContext db) => _db = db;

        public async Task<List<Client>> GetAllAsync() =>
            await _db.Clients
                     .Include(c => c.BusinessArea)
                     .Include(c => c.Requests)
                         .ThenInclude(r => r.Status)
                     .AsNoTracking()
                     .ToListAsync();

        public async Task AddAsync(Client client)
        {
            if (client.BusinessArea != null)
                _db.Attach(client.BusinessArea);

            await _db.Clients.AddAsync(client);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client client)
        {
            _db.Clients.Update(client);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Client client)
        {
            var entity = await _db.Clients.FindAsync(client.Id);
            if (entity != null)
            {
                _db.Clients.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<BusinessArea>> GetBusinessAreasAsync() =>
            await _db.BusinessAreas
                     .OrderBy(b => b.Name)
                     .AsNoTracking()
                     .ToListAsync();
    }
}
