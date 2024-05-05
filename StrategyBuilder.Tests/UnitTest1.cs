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
        public void Startup()
        {
            using(var writer = new StringWriter())
            {
                System.Console.SetOut(writer);
                var host = GameHost.CreateHost(new string[] {});

                var parametrizedThreadStart = new ParameterizedThreadStart((object? obj) =>host.Run());
                var thread = new Thread(parametrizedThreadStart);

                thread.Start();
                Thread.Sleep(5000);
                thread.IsBackground = true;

                string consoleOutput = writer.ToString();
                //Assert.AreEqual("Application started", consoleOutput);
                Assert.That(consoleOutput,Is.EqualTo($"Application started{Environment.NewLine}"));
                //Assert.Pass(consoleOutput);
            }
        }

        [Test]
        public void HelpCommandTest()
        {
            var host = GameHost.CreateHost(new string[] { });

            var parametrizedThreadStart = new ParameterizedThreadStart((object? obj) => host.Run());
            var thread = new Thread(parametrizedThreadStart);

            thread.Start();
            Thread.Sleep(5000);
            thread.IsBackground = true;

            var reader = TextReader
            System.Console.In.
        }
    }
}