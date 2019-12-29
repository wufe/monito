using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monito.Database.Entities;
using Monito.Database.Entities.Interface;

namespace Monito.Database.EFCore
{
    public class MonitoContext : DbContext
    {
        public MonitoContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity => {

                entity.HasKey(x => x.ID);

                entity.ToTable("users");
                entity
                    .Property(x => x.CreatedAt)
                    .HasColumnName("created_at");
                entity
                    .Property(x => x.UpdatedAt)
                    .HasColumnName("updated_at");
            });
                
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving() {
            var entries = ChangeTracker.Entries();
            foreach(var entry in entries) {
                if (entry.Entity is ITimestampTrackedEntity timestampTracked) {
                    var now = DateTime.UtcNow;

                    switch (entry.State) {
                        case EntityState.Modified:
                            timestampTracked.UpdatedAt = now;
                            break;
                        case EntityState.Added:
                            timestampTracked.CreatedAt = now;
                            timestampTracked.UpdatedAt = now;
                            break;
                    }
                }
                if (entry.Entity is IUUIDTrackedEntity uuidTracked) {
                    switch (entry.State) {
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
