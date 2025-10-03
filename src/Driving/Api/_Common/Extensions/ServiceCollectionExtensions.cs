using Microsoft.OpenApi.Models;

namespace Api._Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFcgCatalogApiSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "FCG Catalog API", Version = "v1" });
            });

            return services;
        }
    }
}
