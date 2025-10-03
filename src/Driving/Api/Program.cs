using Api._Common;
using Api._Common.Extensions;
using Api._Common.Middleware;
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
    .AddFcgCatalogApiSwagger();
    
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetCatalogsHandler).Assembly)
).AddValidatorsFromAssemblyContaining<CreateCatalogCommandValidator>(); ;


var elasticSearchSettings = appSettings.ElasticSearchSettings;
services
    .AddElasticSearchModule(elasticSearchSettings);
services
    .AddHealthChecks()
    .AddCheck<OpenSearchHealthCheck>("opensearch", tags: new[] { "search" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FCG Catalog API v1");
        c.RoutePrefix = string.Empty;
    });
    app.MapOpenApi();
}
app
    .UseMiddleware<ExceptionMiddleware>()
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
