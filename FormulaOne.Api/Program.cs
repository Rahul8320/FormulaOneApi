using FormulaOne.Api;
using FormulaOne.Api.Config;
using FormulaOne.DataService.Data;
using FormulaOne.DataService.Repositories;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Services.Caching;
using FormulaOne.Services.Caching.Interface;
using FormulaOne.Services.Notification;
using FormulaOne.Services.Notification.Interface;
using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Get connection string
var dbConnectionString = builder.Configuration.GetConnectionString("DbConnection");
var hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection");
var dbConfig = new DatabaseConfig();
builder.Configuration.GetSection("DatabaseConfig").Bind(dbConfig);

// Add Serilog
builder.Host.UseSerilog((
    context,
    configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Initializing my db context inside the DI container 
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(dbConnectionString, action =>
    {
        action.CommandTimeout(dbConfig.TimeoutTime);
    });

    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors(dbConfig.DetailedError);
        options.EnableSensitiveDataLogging(dbConfig.SensitiveDataLogging);
    }
});

// Add controllers
builder.Services.AddControllers();

// Add auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICachingService, CachingService>();

// Injecting the MediatR to our DI
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(Program).Assembly));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Hangfire Client
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage(hangfireConnectionString));

// Add Hangfire Server
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.UseHangfireDashboard();
app.MapHangfireDashboard("/hangfire");

// RecurringJob.AddOrUpdate(() => Console.WriteLine("Hello from Hangfire!"), "* * * * *");
app.ApplyPendingMigrations();

app.Run();