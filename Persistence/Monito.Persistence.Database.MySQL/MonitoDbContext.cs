using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monito.Persistence.Model;
using Monito.Persistence.Model.Interface;

namespace Monito.Persistence.Database.MySQL
{
    public class MonitoDbContext : DbContext
    {
        public MonitoDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserPersistenceModel>(entity =>
            {
                entity.HasKey(x => x.ID);

                entity.ToTable("users");

                entity
                    .Property(x => x.CreatedAt)
                    .HasColumnName("created_at");
                entity
                    .Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<RequestPersistenceModel>(entity =>
            {
                entity.HasKey(x => x.ID);

                entity.ToTable("requests");

                entity
                    .Property(x => x.CreatedAt)
                    .HasColumnName("created_at");
                entity
                    .Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at");
                entity
                    .Property(x => x.UserID)
                    .HasColumnName("user_id");

                entity
                    .HasOne(r => r.User)
                    .WithMany(u => u.Requests)
                    .HasForeignKey(r => r.UserID);
            });

            modelBuilder.Entity<LinkPersistenceModel>(entity =>
            {
                entity.HasKey(x => x.ID);

                entity.ToTable("links");

                entity
                    .Property(x => x.StatusCode)
                    .HasColumnName("status_code");
                entity
                    .Property(x => x.AdditionalData)
                    .HasColumnName("additional_data");
                entity
                    .Property(x => x.CreatedAt)
                    .HasColumnName("created_at");
                entity
                    .Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at");
                entity
                    .Property(x => x.RedirectsFromLinkId)
                    .HasColumnName("redirects_from_link_id");
                entity
                    .Property(x => x.RequestID)
                    .HasColumnName("request_id");

                entity
                    .HasOne(l => l.RedirectsFrom)
                    .WithOne(l => l.RedirectsTo)
                    .HasForeignKey<LinkPersistenceModel>(l => l.RedirectsFromLinkId);
                entity
                    .HasOne(l => l.Request)
                    .WithMany(r => r.Links)
                    .HasForeignKey(l => l.RequestID);
            });

            modelBuilder.Entity<FilePersistenceModel>(entity =>
            {
                entity.HasKey(x => x.ID);

                entity.ToTable("files");

                entity
                    .Property(x => x.CreatedAt)
                    .HasColumnName("created_at");
                entity
                    .Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at");
                entity
                    .Property(x => x.RequestID)
                    .HasColumnName("request_id");

                entity
                    .HasOne(f => f.Request)
                    .WithMany(r => r.Files)
                    .HasForeignKey(f => f.RequestID);
            });

            modelBuilder.Entity<WorkerPersistenceModel>(entity =>
            {
                entity.HasKey(x => x.ID);

                entity.ToTable("workers");

                entity
                    .Property(x => x.CreatedAt)
                    .HasColumnName("created_at");
                entity
                    .Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<QueuePersistenceModel>(entity =>
            {
                entity.HasKey(x => x.ID);

                entity.ToTable("queues");

                entity
                    .Property(x => x.CreatedAt)
                    .HasColumnName("created_at");
                entity
                    .Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at");
                entity
                    .Property(x => x.WorkerID)
                    .HasColumnName("worker_id");

                entity
                    .HasOne(q => q.Worker)
                    .WithMany(w => w.Queues)
                    .HasForeignKey(q => q.WorkerID);
            });
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is ITimestampTrackedEntity timestampTracked)
                {
                    var now = DateTime.UtcNow;

                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            timestampTracked.UpdatedAt = now;
                            break;
                        case EntityState.Added:
                            timestampTracked.CreatedAt = now;
                            timestampTracked.UpdatedAt = now;
                            break;
                    }
                }
                if (entry.Entity is IUUIDTrackedEntity uuidTracked)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                        case EntityState.Added:
                            if (uuidTracked.UUID == Guid.Empty)
                                uuidTracked.UUID = Guid.NewGuid();
                            break;
                    }
                }
            }
        }
    }
}
