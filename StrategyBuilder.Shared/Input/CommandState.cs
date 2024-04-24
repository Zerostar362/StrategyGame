using StrategyBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StragyBuilder.Shared.Input
{
    public class CommandState
    {
        public string Scope { get; private set; }
        public int CommandCount { get => _commands.Count; }
        private List<ICommand> _commands;
        private int _depth;
        private CommandState? _previousState;


        public CommandState(List<ICommand> commands, string scope,CommandState? previousState = null, int depth = 0)
        {
            _depth = depth;
            _commands = commands;
            Scope = scope;
            _previousState = previousState;
        }

        /// <summary>
        /// Checks if state has command inside its scope of operation
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        private bool HasCommand(string scope)
        {
            foreach (var command in _commands)
            {
                if (command.Flag[_depth] == scope)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to switch the scope of operation
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool TrySwitchToState(string scope, out CommandState? state)
        {
            if (HasCommand(scope))
            {
                var cmds = _commands.Where(cmd => cmd.Flag[_depth] == scope).ToList();
                _depth++;
                /*if (Scope == string.Empty || Scope == ">")
                {
                    state = new CommandState(cmds, $"{scope}");
                    return true;
                }*/

                state = new CommandState(cmds, $"{Scope}{scope} ", this, _depth);
                _depth--;
                return true;
            }

            state = null;
            return false;
        }

        public CommandState? GoBack()
            => _previousState;


        /// <summary>
        /// Prints all commands from scope
        /// </summary>
        public void Help()
        {
            foreach (var command in _commands)
            {
                var sb = new StringBuilder();
                for (var i = _depth; i < command.Flag.Length; i++) 
                {
                    sb.Append(command.Flag[i]);
                    sb.Append(' ');
                }

                sb.Append(Environment.NewLine);
                sb.Append("   ");
                sb.AppendLine(command.Description);
                sb.Append(Environment.NewLine);
                Console.WriteLine(sb.ToString());
            }
        }
    }
}
