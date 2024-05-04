using StragyBuilder.Core;

namespace StrategyBuilder.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            using(var writer = new StringWriter())
            {
                System.Console.SetOut(writer);
                var host = GameHost.CreateHost(new string[] {});

                var parametrizedThreadStart = new ParameterizedThreadStart((object? obj) =>host.Run());
                var thread = new Thread(parametrizedThreadStart);

                thread.Start();
                Thread.Sleep(10000);
                thread.IsBackground = true;

                string consoleOutput = writer.ToString();
                Assert.Pass(consoleOutput);
            }
        }
    }
}