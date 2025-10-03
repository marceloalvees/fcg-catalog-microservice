using Amazon;
using Amazon.Runtime;
using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.ElasticSerach.Repository;
using Microsoft.Extensions.DependencyInjection;
using OpenSearch.Client;
using OpenSearch.Client.JsonNetSerializer;
using OpenSearch.Net;
using OpenSearch.Net.Auth.AwsSigV4;

namespace Infrastructure.ElasticSerach
{
    public static class ElasticSearchModule
    {
        public static IServiceCollection AddElasticSearchModule(
            this IServiceCollection services,
            ElasticSearchSettings settings
        )
        {
            settings.EnsureSettings();

            var connectionSetting = GetConnectionString(settings);

            services.AddSingleton<IOpenSearchClient>(_ => new OpenSearchClient(connectionSetting));

            services.AddScoped<ICatalogRepository, CatalogRepository>();
            services.AddScoped<IGameRepository, GameRepository>();

            return services;
        }

        private static ConnectionSettings GetConnectionString(ElasticSearchSettings settings)
        {

            var credentials = new BasicAWSCredentials(settings.AccessKey, settings.Secret);

            var region = RegionEndpoint.GetBySystemName(settings.Region);

            var awsConnection = new AwsSigV4HttpConnection(credentials, region);

            var pool = new SingleNodeConnectionPool(new Uri(settings.Endpoint));

            var connectionSetting = new ConnectionSettings(pool, awsConnection, sourceSerializer: JsonNetSerializer.Default)
                .DefaultMappingFor<Catalog>(g => g
                    .IdProperty(p => p.Key)
                    .IndexName(settings.IndexName)
                );

            return connectionSetting;
        }

        private static ElasticSearchSettings EnsureSettings(this ElasticSearchSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            ArgumentException.ThrowIfNullOrWhiteSpace(settings.Endpoint);

            ArgumentException.ThrowIfNullOrWhiteSpace(settings.AccessKey);

            ArgumentException.ThrowIfNullOrWhiteSpace(settings.Secret);

            ArgumentException.ThrowIfNullOrWhiteSpace(settings.Region);

            ArgumentException.ThrowIfNullOrWhiteSpace(settings.IndexName);

            return settings;
        }
    }
}
