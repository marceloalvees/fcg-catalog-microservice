using Infrastructure.ElasticSerach;

namespace Api._Common
{
    public class AppSettings
    {
        public required ElasticSearchSettings ElasticSearchSettings { get; set; }
    }
}
