using ClientWpf.Data;
using ClientWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ClientWpf.Services
{
    public class RequestService
    {
        /// <summary>
        /// Выполняет CRUD-операции и загрузку связанных данных (клиентов и статусов).
        /// Для каждой операции создаёт новый экземпляр <see cref="AppDbContext"/>,
        /// чтобы избежать ошибок при многопоточном доступе к контексту Entity Framework.
        /// </summary>
        private readonly DbContextOptions<AppDbContext> _options;

        public RequestService()
        {
            var conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(conn);
            _options = optionsBuilder.Options;
        }

        private AppDbContext CreateContext()
        {
            return new AppDbContext(_options);
        }

        public async Task<List<Request>> GetByClientIdAsync(int clientId)
        {
            using (var db = CreateContext())
            {
                return await db.Requests
                    .Include(r => r.Status)
                    .Include(r => r.Client)
                    .Where(r => r.ClientId == clientId)
                    .OrderByDescending(r => r.RequestDate)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task AddAsync(Request request)
        {
            using (var db = CreateContext())
            {
                await db.Requests.AddAsync(request);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Request request)
        {
            using (var db = CreateContext())
            {
                request.Client = null;
                request.Status = null;

                db.Requests.Attach(request);
                db.Entry(request).Property(r => r.StatusId).IsModified = true;

                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Request request)
        {
            using (var db = CreateContext())
            {
                // Подтягиваем сущность из БД, чтобы EF знал, что это tracked
                var entity = await db.Requests.FindAsync(request.Id);
                if (entity != null)
                {
                    db.Requests.Remove(entity);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<List<Request>> GetAllAsync()
        {
            using (var db = CreateContext())
            {
                return await db.Requests
                    .Include(r => r.Status)
                    .Include(r => r.Client)
                    .OrderByDescending(r => r.RequestDate)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        public async Task<List<RequestStatus>> GetStatusesAsync()
        {
            using (var db = CreateContext())
            {
                return await db.RequestStatuses
                    .OrderBy(s => s.Name)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }
    }
}
