using Mailer.Core.Data;
using Mailer.Core.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Mailer.Tests.Data;

public class EmailDbContextTests : IAsyncLifetime
{
    private EmailDbContext _context = null!;

    public async Task InitializeAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        // By passing an open connection, the DbContext will not automatically close the connection.
        // This is important as closing the connection just deletes the in-memory database.
        var options = new DbContextOptionsBuilder()
            .UseSqlite(connection)
            .Options;
        _context = new EmailDbContext(options);
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.Database.CloseConnectionAsync();
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task NewMessages_HaveCorrectCreatedAndLastModifiedTime()
    {
        var message = new EmailMessage();
        await _context.Messages.AddAsync(message);
        var beforeSave = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        var afterSave = DateTime.UtcNow;

        Assert.InRange(message.CreatedOn, beforeSave, afterSave);
        // CreationOn and LastModifiedOn should be exactly the same on new messages.
        Assert.Equal(message.CreatedOn, message.LastModifiedOn);
    }

    [Fact]
    public async Task UpdatedMessages_HaveOnlyLastModifiedOnUpdated()
    {
        var message = new EmailMessage();
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
        var previousCreatedOn = message.CreatedOn;

        message.Subject = "A Change Occured";
        var beforeSave = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        var afterSave = DateTime.UtcNow;

        Assert.InRange(message.LastModifiedOn, beforeSave, afterSave);
        Assert.NotEqual(message.CreatedOn, message.LastModifiedOn);
        // The CreatedOn field did *not* change due to modifications
        Assert.Equal(previousCreatedOn, message.CreatedOn);
    }
}
