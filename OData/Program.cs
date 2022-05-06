using Mailer.Core.Data;
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
    // Migrate the database
    using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<EmailDbContext>();
    await db.Database.MigrateAsync();
}

app.UseODataQueryRequest();
app.UseRouting();
app.MapControllers();

app.Run();
