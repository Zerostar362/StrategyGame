using System.Windows.Input;

namespace StragyBuilder.Shared.Input
{
    public class Command : ICommand
    {
        private Action<object?> _execute;
        private Func<object?, bool> _canExecute;

        public Command(Action<object?> execute, Func<object?, bool> canExecute)
        {
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
