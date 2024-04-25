namespace StragyBuilder.Shared.Input
{
    public class Command : StrategyBuilder.Interfaces.ICommand
    {
        private Action<object?> _execute;
        private Func<object?, bool> _canExecute;

        public string[] Flag { get; init; }
        public string Description { get; init; }

        public Command(string flag, string description, Action<object?> execute, Func<object?, bool> canExecute)
        {
            Flag = flag.ToLower().Split(" ");
            Description = description;
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? false;
        }

        public void Execute(object? parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
