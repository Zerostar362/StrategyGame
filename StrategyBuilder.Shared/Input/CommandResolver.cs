using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StrategyBuilder.Interfaces;

namespace StragyBuilder.Shared.Input
{
    public class CommandResolver : IHostedService
    {
        private List<ICommand> commands = new();
        private CancellationTokenSource cancellationTokenSource = new();
        private Task Loop { get; set; }

        private CommandContext _commandContext;

        public CommandResolver(IServiceProvider provider)
        {
            commands = provider.GetServices<ICommand>()?.ToList() ?? throw new NullReferenceException("No commands inserted");

            _commandContext = new CommandContext(new CommandState(commands, ""));
        }

        private void HelpCommand(object? obj)
        {
            commands.ForEach(cmd => Console.WriteLine(cmd.Flag));
        }

        public void Resolve(string[] scopes, string[] parameters)
        {
            try
            {


                if (scopes[0] == "help")
                {
                    _commandContext.PrintHelp();
                    return;
                }

                if (scopes[0] == "..")
                {
                    _commandContext.Goback();
                    return;
                }



                foreach (var scope in scopes)
                {
                    _commandContext.SwitchOrExecute(scope, parameters);
                }
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("ERROR:");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" ");
                Console.WriteLine(ex.Message);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Type Help or just press enter for list of commands");
            Loop = Task.Run(() =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {


                        Console.Write(_commandContext.PrintState());
                        var input = Console.ReadLine()?.ToLower();
                        if (input is null || input == string.Empty)
                            input = "help";

                        var inputSplit = input.Split(' ');

                        List<string> scope = new();
                        List<string> args = new();
                        bool argsStarted = false;
                        for (int i = 0; i < inputSplit.Length; i++)
                        {
                            if (!inputSplit[i].Contains("-") && argsStarted)
                                throw new ArgumentException("You cannot add scope variable after parameter argument");

                            //todo modify to support multiparameter
                            if (inputSplit[i].Contains("-"))
                            {
                                args.Add(inputSplit[i]);
                                //need to skip the gap " "
                                i++;
                                args.Add(inputSplit[i]);
                                continue;
                            }

                            scope.Add(inputSplit[i]);
                        }

                        Resolve(scope.ToArray(), args.ToArray());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Invalide command");
                    }
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource.Cancel();
            Loop?.Wait();
            return Task.CompletedTask;
        }
    }
}
