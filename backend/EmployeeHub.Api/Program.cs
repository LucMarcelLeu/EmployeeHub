using Serilog;
using EmployeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EmployeeHub.Application.Employees.Interfaces;
using EmployeeHub.Infrastructure.Employees;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication;
using EmployeeHub.Api.Security;

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

builder.Services.AddSwaggerGen(c =>
{
    // 1. Definition für das JWT Bearer Token festlegen
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(document =>
        {
            return new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
            };
        });
});

builder.Services.AddHealthChecks();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8080/realms/employeehub";

        options.RequireHttpsMetadata = false;

        options.Audience = "employeehub-api";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            NameClaimType = "preferred_username",
            RoleClaimType = ClaimTypes.Role
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var identity = context.Principal?.Identity as ClaimsIdentity;

                var realmAccess = context.Principal?
                    .FindFirst("realm_access")?
                    .Value;

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddTransient<IClaimsTransformation, KeycloakRoleClaimsTransformation>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/employeehub-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

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


app.UseRouting();

app.UseCors("AllowAngular");

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();