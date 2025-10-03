using UnitTests.Factories;
using Xunit;

namespace UnitTests._Common
{
    public abstract class TestBase(FCGFixture fixture) : IClassFixture<FCGFixture>
    {
        protected CancellationToken CancellationToken { get; } = fixture.CancellationToken;

        protected static ModelFactory ModelFactory { get; } = new();
    }
}
