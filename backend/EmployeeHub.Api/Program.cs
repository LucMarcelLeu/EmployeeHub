using Serilog;
using EmployeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EmployeeHub.Application.Employees.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication;
using EmployeeHub.Api.Security;
using EmployeeHub.Application.Departments.Interfaces;
using EmployeeHub.Infrastructure.Services;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmployeeHubDbContext>(
    options =>
        options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            // Aktiviert automatische Wiederholungsversuche bei Fehlern beim Start
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,        // Anzahl der Versuche
                maxRetryDelay: TimeSpan.FromSeconds(10), // Max. Wartezeit dazwischen
                errorNumbersToAdd: null);
        }));

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
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

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
        var authority = builder.Configuration["Authentication:Authority"] ?? "http://localhost:8080/realms/employeehub";
        var metadataAddress = builder.Configuration["Authentication:MetadataAddress"];

        // Wenn in Docker die MetadataAddress gesetzt wurde, wird sie hier zugewiesen
        if (!string.IsNullOrEmpty(metadataAddress))
        {
            options.MetadataAddress = metadataAddress;
        }
        options.Authority = authority;
        options.Audience = "employeehub-api";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:8080/realms/employeehub",

            ValidateAudience = true,
            ValidAudience = "employeehub-api",

            NameClaimType = "preferred_username",
            RoleClaimType = "roles",
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                // Hilft dir beim Debuggen im Docker-Log zu sehen, WARUM es fehlschlägt
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var identity = context.Principal?.Identity as ClaimsIdentity;
                if (identity is null) return Task.CompletedTask;

                // realm_access Claim auslesen (JSON-String)
                var realmAccessJson = context.Principal?.FindFirst("realm_access")?.Value;
                if (!string.IsNullOrEmpty(realmAccessJson))
                {
                    using var doc = JsonDocument.Parse(realmAccessJson);
                    if (doc.RootElement.TryGetProperty("roles", out var roles))
                    {
                        foreach (var role in roles.EnumerateArray())
                        {
                            identity.AddClaim(new Claim("roles", role.GetString()!));
                        }
                    }
                }

                Console.WriteLine($"Token validated. Roles added: {string.Join(",", identity.Claims.Where(c => c.Type == "roles").Select(c => c.Value))}");

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