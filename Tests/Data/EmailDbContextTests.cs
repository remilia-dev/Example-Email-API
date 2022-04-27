using Mailer.Core.Data;
using Mailer.Core.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public async Task EmailMessage_CreatedOn_HasCorrectValueForNewRecords()
    {
        var message = BlankEmailMessage();
        await _context.Messages.AddAsync(message);
        var beforeSave = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        var afterSave = DateTime.UtcNow;

        Assert.InRange(message.CreatedOn, beforeSave, afterSave);
    }

    [Fact]
    public async Task EmailMessage_CreatedOn_DoesNotChangeOnModifiedRecords()
    {
        var message = BlankEmailMessage();
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        var previousCreatedOn = message.CreatedOn;
        message.WasSent = false;
        await _context.SaveChangesAsync();

        Assert.Equal(previousCreatedOn, message.CreatedOn);
    }

    [Fact]
    public async Task EmailMessage_CreatedOn_ThrowsOnSaveIfChanged()
    {
        var message = BlankEmailMessage();
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        var previousCreatedOn = message.CreatedOn;
        message.CreatedOn = DateTime.MaxValue;

        await Assert.ThrowsAsync<InvalidOperationException>(()
            => _context.SaveChangesAsync());
    }

    [Fact]
    public async Task EmailMessage_LastModifiedOn_SameAsCreatedOnForNewRecords()
    {
        var message = BlankEmailMessage();
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        Assert.Equal(message.CreatedOn, message.LastModifiedOn);
    }

    [Fact]
    public async Task EmailMessage_LastModifiedOn_UpdatedOnModifiedRecords()
    {
        var message = BlankEmailMessage();
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        message.WasSent = false;
        var beforeSave = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        var afterSave = DateTime.UtcNow;

        Assert.InRange(message.LastModifiedOn, beforeSave, afterSave);
    }

    private static EmailMessage BlankEmailMessage()
    {
        return new EmailMessage()
        {
            Sender = "",
            Subject = "",
            HtmlBody = "",
            Recipients = new List<EmailRecipient>(),
        };
    }
}
