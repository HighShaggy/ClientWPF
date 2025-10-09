using ClientWpf.Data;
using ClientWpf.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace ClientWpf.Services
{
    /// <summary>
    /// Для асинхронных операций создаёт отдельный экземпляр <see cref="AppDbContext"/>,
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly AppDbContext _db;
        public event Action ClientsChanged;

        public ClientService(AppDbContext db)
        {
            _db = db;
        }

        private AppDbContext CreateContext()
        {
            var connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connString);
            return new AppDbContext(optionsBuilder.Options);
        }

        public async Task<List<Client>> GetAllAsync()
        {
            using (var db = CreateContext())
            {
                return await db.Clients
                    .Include(c => c.BusinessArea)
                    .Include(c => c.Requests)
                        .ThenInclude(r => r.Status)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task AddAsync(Client client)
        {
            using (var db = CreateContext())
            {
                if (client.BusinessArea != null)
                    db.Attach(client.BusinessArea);

                await db.Clients.AddAsync(client);
                await db.SaveChangesAsync();
            }

            ClientsChanged?.Invoke();
        }

        public async Task UpdateAsync(Client client)
        {
            using (var db = CreateContext())
            {
                db.Clients.Update(client);
                await db.SaveChangesAsync();
            }

            ClientsChanged?.Invoke();
        }

        public async Task DeleteAsync(Client client)
        {
            using (var db = CreateContext())
            {
                var entity = await db.Clients.FindAsync(client.Id);
                if (entity != null)
                {
                    db.Clients.Remove(entity);
                    await db.SaveChangesAsync();
                }
            }

            ClientsChanged?.Invoke();
        }

        public async Task<List<BusinessArea>> GetBusinessAreasAsync()
        {
            using (var db = CreateContext())
            {
                return await db.BusinessAreas
                    .OrderBy(b => b.Name)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }
    }
}
