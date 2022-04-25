using Mailer.Core.Data;
using Mailer.Core.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Mailer.Tests.Data;

public class MessageDbContextTests
{
    private readonly MessageDbContext _context;

    public MessageDbContextTests()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder()
            .UseSqlite(connection)
            .Options;
        _context = new MessageDbContext(options);
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task NewMessages_HaveCorrectCreatedAndLastModifiedTime()
    {
        var message = new Message();
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
        var message = new Message();
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
