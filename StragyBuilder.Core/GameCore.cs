using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StragyBuilder.Core
{
    public partial class GameCore : IHostedService
    {
        private ILogger<GameCore> _logger;
        private IServiceProvider ServiceProvider { get; }
        private CancellationTokenSource CancellationTokenSource { get; }


        private Task _updateLoop;

        public GameCore(ILogger<GameCore> logger, IServiceProvider provider)
        {
            _logger = logger;
            ServiceProvider = provider;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _updateLoop = Task.Run(() =>
            {
                while (!CancellationTokenSource.Token.IsCancellationRequested)
                {
                    Update();
                    Thread.Sleep(500);
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            CancellationTokenSource.Cancel();
            _updateLoop?.Wait();
            return Task.CompletedTask;
        }

        public void Update()
        {

        }
    }
}
