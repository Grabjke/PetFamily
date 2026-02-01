using System.Reflection;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using MassTransit;
using Microsoft.OpenApi.Models;
using PetFamily.Accounts.Infrastructure.Consumers;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Web;
using PetFamily.Web.Middlewares;
using Serilog;
using Serilog.Events;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

string indexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Elasticsearch([new Uri("http://localhost:9200")], options =>
    {
        options.DataStream = new DataStreamName(indexFormat);
        options.TextFormatting = new EcsTextFormatterConfiguration<LogEventEcsDocument>();
        options.BootstrapMethod = BootstrapMethod.Silent;
    })
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
                },
            },
            []
        },
    });
});

builder.Services.AddSerilog();

builder.Services
    .AddProgramDependency(builder.Configuration);

builder.Services.AddStackExchangeRedisCache(options =>
{
    string connection = builder.Configuration.GetConnectionString("Redis") ??
                        throw new ArgumentNullException(nameof(connection));

    options.Configuration = connection;
});

builder.Services.AddMassTransit(configure =>
{
    configure.SetKebabCaseEndpointNameFormatter();

    configure.AddConsumer<RegisterVolunteerConsumer>();

    configure.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration["RabbitMQ:Host"]!), h =>
        {
            h.Username(builder.Configuration["RabbitMQ:UserName"]!);
            h.Password(builder.Configuration["RabbitMQ:Password"]!);
        });

        cfg.Durable = true;
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();


var seeder = app.Services.GetService<AccountsSeeder>();

await seeder!.SeedAsync();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // await app.ApplyMigration();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(config =>
{
    config.WithOrigins("http://localhost:5173", "http://localhost:5174")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

namespace PetFamily.Web
{
    public partial class Program;
}