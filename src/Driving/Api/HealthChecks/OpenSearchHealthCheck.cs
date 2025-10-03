﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenSearch.Client;

namespace Api.HealthChecks
{
    public class OpenSearchHealthCheck(IOpenSearchClient client) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken ct = default
        )
        {
            try
            {
                var pingResponse = await client.PingAsync(ct: ct);

                if (pingResponse.IsValid)
                    return HealthCheckResult.Healthy("OpenSearch is available.");

                return HealthCheckResult.Unhealthy("OpenSearch ping failed", pingResponse.OriginalException);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("OpenSearch health check exception", ex);
            }
        }
    }
}
