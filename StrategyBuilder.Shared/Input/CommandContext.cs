namespace StragyBuilder.Shared.Input
{
    public class CommandContext
    {
        private CommandState _state;
        private CommandState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public CommandContext(CommandState state)
        {
            _state = state;
        }

        public string PrintState()
        {
            return _state.Scope;
        }

        public void SwitchOrExecute(string scope, string[] parameters)
        {
            if (_state.CommandCount == 1)
            {
                if (!_state.TryExecute(parameters, out var msg))
                {
                    Console.WriteLine(msg);
                }

                return;
            }

            if (!_state.TrySwitchToState(scope, out var state))
                throw new ArgumentException($"No such scope as {scope}");
            
            _state = state ?? throw new NullReferenceException("State came back as null");
        }

        public void PrintHelp() =>
            _state.Help();

        public void Goback()
        {
            var state = _state.GoBack();
            if (state is null)
                return;

            _state = state;
        }
    }
}
