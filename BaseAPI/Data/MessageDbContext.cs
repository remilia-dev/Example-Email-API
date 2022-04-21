using BaseAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BaseAPI.Data
{
    public class MessageDbContext : DbContext
    {
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Recipient> Recipients => Set<Recipient>();

        public MessageDbContext(DbContextOptions options) : base(options) { }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BeforeSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected virtual void BeforeSaveChanges()
        {
            foreach (var entity in ChangeTracker.Entries<Message>())
            {
                var message = entity.Entity;
                switch (entity.State)
                {
                    case EntityState.Added:
                        message.CreatedOn = DateTime.UtcNow;
                        message.LastModifiedOn = message.CreatedOn;
                        break;
                    case EntityState.Modified:
                        message.LastModifiedOn = DateTime.UtcNow;
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .Property(e => e.CreatedOn)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<Recipient>()
                .Property(e => e.Type)
                .HasConversion<int>();
        }
    }
}
