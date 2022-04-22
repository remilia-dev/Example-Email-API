using BaseAPI.Data;
using BaseAPI.Model;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Resources.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<MessageDbContext>(options =>
{
    // TODO:
    options.UseSqlite("Data Source=test.db");
#if DEBUG
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
#endif
});

builder.Services.AddJsonApi<MessageDbContext>(options =>
{
    options.Namespace = "api/v1";
    options.DefaultAttrCapabilities = AttrCapabilities.AllowView
        | AttrCapabilities.AllowFilter
        | AttrCapabilities.AllowSort
        | AttrCapabilities.AllowCreate;
    options.MaximumPageSize = new PageSize(100);

    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.WriteIndented = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseJsonApi();

app.MapControllers();
await CreateDatabaseAsync(app.Services);

app.Run();

static async Task CreateDatabaseAsync(IServiceProvider services)
{
    await using var scope = services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<MessageDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

    if (!dbContext.Messages.Any())
    {
        var message = new Message()
        {
            Sender = "example@example.org",
            Subject = "Hallo",
            Recipients = new List<Recipient>()
            {
                new Recipient("to@example.org"),
                new Recipient("to_someone@example.org"),
                new Recipient("cc@example.org", RecipientType.Cc),
            },
        };
        dbContext.Messages.Add(message);
        await dbContext.SaveChangesAsync();
    }
}