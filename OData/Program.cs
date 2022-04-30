using Mailer.Core.Data;
using Mailer.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;
using Mailer.OData;
using Mailer.Core.Services;
using Mailer.MailKit;

var builder = WebApplication.CreateBuilder(args);
// Add Services
builder.Services.AddDbContext<EmailDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("EmailDatabase");
    options.UseSqlServer(connectionString);
    options.UseLazyLoadingProxies();
#if DEBUG
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
#endif
});
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("api/v1", EdmModelBuilder.GetV1Model()));

// Email Services
builder.Services.AddOptions<SmtpConnectionOptions>()
    .BindConfiguration("SmtpConnection")
    .ValidateDataAnnotations()
    .ValidateOnStart()
    .PostConfigure(options =>
    {
#if !DEBUG
        // Ensure sensitive data logging is turned off in non-debug builds.
        options.LogSensitiveData = false;
#endif
    });
builder.Services.AddScoped<IEmailTransport, SmtpEmailTransport>();
builder.Services.AddSingleton<IEmailQueue, EmailQueue>();
builder.Services.AddHostedService<BackgroundEmailService>();

var app = builder.Build();
// Configure Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseODataRouteDebug();
}
app.UseODataQueryRequest();
app.UseRouting();

app.MapControllers();

await CreateDatabaseAsync(app.Services);

app.Run();

// TODO: Move Database Creation/Seeding
static async Task CreateDatabaseAsync(IServiceProvider services)
{
    await using var scope = services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<EmailDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

    if (!dbContext.Messages.Any())
    {
        var message = new EmailMessage()
        {
            Sender = "example@example.org",
            Subject = "Hallo",
            Recipients = new List<EmailRecipient>()
            {
                new EmailRecipient("to@example.org"),
                new EmailRecipient("to_someone@example.org"),
                new EmailRecipient("cc@example.org", RecipientType.Cc),
            },
        };
        dbContext.Messages.Add(message);
        await dbContext.SaveChangesAsync();
    }
}
