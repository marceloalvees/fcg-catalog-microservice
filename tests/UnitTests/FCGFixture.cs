

namespace UnitTests
{
    public class FCGFixture : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public FCGFixture()
        {
            CancellationToken = _cancellationTokenSource.Token;
        }

        public CancellationToken CancellationToken { get; private set; }

        public void Dispose() => _cancellationTokenSource.Cancel();
    }
}
