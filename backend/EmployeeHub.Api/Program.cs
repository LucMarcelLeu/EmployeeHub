using Serilog;
using EmployeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EmployeeHub.Application.Employees.Interfaces;
using EmployeeHub.Infrastructure.Employees;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmployeeHubDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration
            .GetConnectionString("DefaultConnection")));

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
        .GetRequiredService<EmployeeHubDbContext>();

    await DbInitializer.InitializeAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();