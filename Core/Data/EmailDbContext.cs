using Mailer.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Mailer.Core.Data;

public class EmailDbContext : DbContext
{
    public DbSet<EmailMessage> Messages => Set<EmailMessage>();
    public DbSet<EmailRecipient> Recipients => Set<EmailRecipient>();

    public EmailDbContext(DbContextOptions options) : base(options) { }

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
        foreach (var entity in ChangeTracker.Entries<EmailMessage>())
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
        modelBuilder.Entity<EmailMessage>()
            .Property(e => e.CreatedOn)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        modelBuilder.Entity<EmailRecipient>()
            .Property(e => e.Type)
            .HasConversion<int>();
    }
}
