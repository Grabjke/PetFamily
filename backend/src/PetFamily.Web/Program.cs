using Microsoft.OpenApi.Models;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Accounts.Presentation.DependencyInjection;
using PetFamily.Core.Extensions;
using PetFamily.Files.Application;
using PetFamily.Species.Presentation.DependencyInjection;
using PetFamily.Volunteers.Presentation.DependencyInjection;
using PetFamily.Web.Middlewares;
using Serilog;
using Serilog.Events;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")
                 ?? throw new ArgumentNullException("Seq"))
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

builder.Services.AddSerilog();

builder.Services
    .AddVolunteersModule(builder.Configuration)
    .AddSpeciesModule(builder.Configuration)
    .AddAccountsModule(builder.Configuration)
    .AddFilesApplication();

var app = builder.Build();

var seeder = app.Services.GetService<AccountsSeeder>();

await seeder.SeedAsync();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //  await app.ApplyMigration();
}

app.UseHttpsRedirection();

app.UseCors(config =>
{
    config.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/api/users", () =>
{
    List<string> users = ["user1", "user2", "user3"];
    return Results.Ok(users);
});

app.Run();

namespace PetFamily.Web
{
    public partial class Program;
}