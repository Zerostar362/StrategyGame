namespace StrategyBuilder.Interfaces
{
    public interface ICommand : System.Windows.Input.ICommand
    {
        string[] Flag { get; }
        string Description { get; init; }
    }
}
