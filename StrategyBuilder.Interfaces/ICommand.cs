namespace StrategyBuilder.Interfaces
{
    /// <summary>
    /// Use System.Windows.Input.ICommand instead
    /// </summary>
    [Obsolete]
    public interface ICommand : System.Windows.Input.ICommand
    {
        //todo there are still classes using this fucking shit. REMOVE IT
        string[] Flag { get; }
        string Description { get; init; }
    }
}
