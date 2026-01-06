using Infrastructure.ElasticSerach;

namespace Api._Common.Settings
{
    public class AppSettings
    {
        public required ElasticSearchSettings ElasticSearchSettings { get; set; }

        public required AuthenticationSettings AuthenticationSettings { get; set; }
    }
}
