using Api._Common.Extensions;
using Api._Common.Middleware;
using Api._Common.Settings;
using Api.HealthChecks;
using Application.Handler.Catalogs.Queries.GetCatalogs;
using Application.Validators;
using FluentValidation;
using HealthChecks.UI.Client;
using Infrastructure.ElasticSerach;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;

var appSettings = builder.Configuration
    .Get<AppSettings>();

ArgumentNullException.ThrowIfNull(appSettings);

services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
services.AddOpenApi()
    .AddFcgCatalogApiSwagger(appSettings.AuthenticationSettings);
    
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetCatalogsHandler).Assembly)
).AddValidatorsFromAssemblyContaining<CreateCatalogCommandValidator>(); ;

services
    .ConfigureAuthentication(appSettings.AuthenticationSettings)
    .ConfigureAuthorization();

var elasticSearchSettings = appSettings.ElasticSearchSettings;
services
    .AddElasticSearchModule(elasticSearchSettings);
services
    .AddHealthChecks()
    .AddCheck<OpenSearchHealthCheck>("opensearch", tags: new[] { "search" });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FCG Catalogs API");

        c.OAuthClientId(appSettings.AuthenticationSettings.Audience);
        c.OAuthAppName("FCG Catalogs API - Swagger");
        c.OAuthUsePkce();
    });

    app.MapOpenApi();
}

app
    .UseMiddleware<ExceptionMiddleware>()
    .UseAuthentication()
    .UseAuthorization()
    .UseHttpMetrics();

app.MapHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter
            .WriteHealthCheckUIResponse
    }
);
app.MapMetrics("/metrics");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
