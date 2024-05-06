using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StragyBuilder.Core;
using StrategyBuilder.Console.System;
using StrategyBuilder.ConsoleController.Core;
using System.Windows.Input;

namespace StrategyBuilder.Tests
{
    public class Tests
    {
        private GameHost gameHost;
        class ConsoleTestInput : TextReader
        {

        }
        [SetUp]
        public void Setup()
        {
            var host = GameHost.CreateHost(new string[] { });
            var parametrizedThreadStart = new ParameterizedThreadStart((object? obj) => host.Run());
            var thread = new Thread(parametrizedThreadStart);

            thread.Start();
            Thread.Sleep(5000);
            thread.IsBackground = true;

            gameHost = host;
        }

        [Test]
        public void Startup()
        {
            using (var writer = new StringWriter())
            {
                System.Console.SetOut(writer);
                var host = GameHost.CreateHost(new string[] { });

                var parametrizedThreadStart = new ParameterizedThreadStart((object? obj) => host.Run());
                var thread = new Thread(parametrizedThreadStart);

                thread.Start();
                Thread.Sleep(5000);
                thread.IsBackground = true;

                string consoleOutput = writer.ToString();
                //Assert.AreEqual("Application started", consoleOutput);
                Assert.That(consoleOutput, Is.EqualTo($"Application started{Environment.NewLine}"));
                //Assert.Pass(consoleOutput);
            }
        }

        [Test]
        public void HelpCommandTest()
        {
            using (var writer = new StringWriter())
            {
                System.Console.SetOut(writer);
                var originalInput = "help";
                var host = GameHost.CreateHost(new string[] { });
                var parametrizedThreadStart = new ParameterizedThreadStart((object? obj) => host.Run());
                var thread = new Thread(parametrizedThreadStart);

                thread.Start();
                thread.IsBackground = true;
                Thread.Sleep(5000);

                var sysCmd = host.Host.Services.GetService<SystemCommand>();
                var cmds = host.Host.Services.GetService<IDictionary<string, ICommand>>();

                sysCmd.CheckAndTryExecute(originalInput, null, null);


                var writerStr = writer.ToString();
                var checkArr = new Dictionary<string, bool>();
                var split = writerStr.Split(Environment.NewLine);

                foreach (var cmd in cmds)
                {
                    if (split.Contains(cmd.Key))
                        checkArr.Add(cmd.Key, true);
                }




                Assert.That(() =>
                {
                    foreach (var check in checkArr)
                    {
                        var isMissing = false;
                        foreach (var cmd in cmds)
                        {
                            if (check.Key == cmd.Key)
                            {
                                isMissing = false;
                                break;
                            }

                            isMissing = true;
                        }

                        if (isMissing == true)
                            return false;

                        return true;
                    }

                    return false;
                });
            }
        }

        [Test]
        public void ChangeDirectory_Resource()
        {
            using(var writer = new StringWriter()) 
            {
                System.Console.SetOut(writer);
                var sysCmd = gameHost.Host.Services.GetService<SystemCommand>();
                var cmds = gameHost.Host.Services.GetService<IDictionary<string, ICommand>>();
                var context = gameHost.Host.Services.GetService<ConsoleEnvironmentContext>();

                sysCmd.CheckAndTryExecute("cd", new string[] { "resource" }, new string[] {"resource"});

                var output = writer.ToString();

                Assert.That(output, Is.EqualTo($"resource>{Environment.NewLine}"));
                
            }
        }

        [Test]
        public void ChangeDirectory_Building()
        {

        }
    }
}