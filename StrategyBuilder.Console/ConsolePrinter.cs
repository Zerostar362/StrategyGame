using Microsoft.Extensions.Hosting;

namespace StrategyBuilder.Console
{
    public class ConsolePrinter : IHostedService
    {
        private CancellationTokenSource _cancellationTokenSource = new();


        public ConsolePrinter()
        {

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void MainLoop()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {

            }
        }
    }
}
