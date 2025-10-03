namespace IntegrationTests._Common
{
    public abstract class ControllerTestBase
    {
        protected readonly FcgFixture Fixture;
        protected readonly string Url;

        protected ControllerTestBase(FcgFixture fixture, string url)
        {
            Fixture = fixture;
            Url = url;
        }
    }
}
