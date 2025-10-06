using ClientWpf.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ClientWpf.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base(GetOptions()) { }

        public virtual DbSet<BusinessArea> BusinessAreas { get; set; }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<Request> Requests { get; set; }

        public virtual DbSet<RequestStatus> RequestStatuses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        private static DbContextOptions<AppDbContext> GetOptions()
        {
            var conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(conn);
            return optionsBuilder.Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusinessArea>(entity =>
            {
                entity.ToTable("business_areas");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .IsRequired();
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .IsRequired();

                entity.Property(e => e.Inn)
                      .HasColumnName("inn")
                      .IsRequired()
                      .HasMaxLength(12);

                entity.Property(e => e.BusinessAreaId)
                      .HasColumnName("business_area_id");

                entity.Property(e => e.Note)
                      .HasColumnName("note");

                entity.HasOne(e => e.BusinessArea)
                      .WithMany(b => b.Clients)
                      .HasForeignKey(e => e.BusinessAreaId);
            });

            modelBuilder.Entity<RequestStatus>(entity =>
            {
                entity.ToTable("request_statuses");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Name)
                      .HasColumnName("name")
                      .IsRequired()
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("requests");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.ClientId)
                      .HasColumnName("client_id");

                entity.Property(e => e.RequestDate)
                      .HasColumnName("request_date")
                      .IsRequired();

                entity.Property(e => e.WorkName)
                      .HasColumnName("work_name")
                      .IsRequired();

                entity.Property(e => e.WorkDescription)
                      .HasColumnName("work_description");

                entity.Property(e => e.StatusId)
                      .HasColumnName("status_id");

                entity.HasOne(e => e.Client)
                      .WithMany(c => c.Requests)
                      .HasForeignKey(e => e.ClientId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Status)
                      .WithMany(s => s.Requests)
                      .HasForeignKey(e => e.StatusId);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
